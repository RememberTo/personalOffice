using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.User.Commands.UpdateUserProfile;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entites.User;
using PersonalOffice.Backend.Domain.Entities.User;
using PersonalOffice.Backend.Domain.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using Convert = PersonalOffice.Backend.Application.Common.Global.Convert;

namespace PersonalOffice.Backend.Application.CQRS.User.Commands.UpdateProfile
{
    internal class UpdateUserProfileCommandHandler(
        ILogger<UpdateUserProfileCommandHandler> logger,
        ITransportService transportService,
        IUserService userService) : IRequestHandler<UpdateUserProfileCommand, UserProfile>
    {
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IUserService _userService = userService;

        public async Task<UserProfile> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var oldProfile = await _userService.GetUserProfileAsync(request.UserId, cancellationToken);

            oldProfile.Subscriptions = await ChangeSubscriptions(request.UserId, request.UpdateProfile.Subscriptions, oldProfile.Subscriptions, cancellationToken);

            if(request.UpdateProfile.IsTwoFactor != oldProfile.IsTwoFactor)
            {
                _logger.LogTrace("{stat} двухфакторной аутентификации", oldProfile.IsTwoFactor ? "Отключение" : "Подключение");

                var msg = (await _transportService.RPCServiceAsync(new Message
                {
                    Source = MicroserviceNames.Backend,
                    Destination = MicroserviceNames.DbConnector,
                    Method = "PO_SetTwoFactorAuth",
                    Data = new SetTwoFactAuthCommand { UserId = request.UserId, IsTwoFactAuth = request.UpdateProfile.IsTwoFactor},
                }, cancellationToken));

                var sqlResult = Convert.DataTo<SQLOperationResult<object>>(msg.Data);

                if (sqlResult.Success) oldProfile.IsTwoFactor = request.UpdateProfile.IsTwoFactor;
            }

            if(request.UpdateProfile.IsPassTroughAuth != oldProfile.IsPassTroughAuth)
            {
                _logger.LogTrace("Измененеие бесшовного перехода между приложениями");

                await _userService.ChangeRoleAsync(new ChangeRoleRequest
                {
                    UserID = request.UserId,
                    RoleID = 10,
                    Action = request.UpdateProfile.IsPassTroughAuth ? RoleAction.Set : RoleAction.Drop
                }, cancellationToken);

                oldProfile.IsPassTroughAuth = request.UpdateProfile.IsPassTroughAuth;
            }

            return oldProfile;
        }

        private async Task<ICollection<SubscriptionNotifyInfo>> ChangeSubscriptions(int userId, ICollection<SubscriptionNotifyInfo> newSubscriptions, 
            ICollection<SubscriptionNotifyInfo> oldSubscriptions, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало изменения продписок");

            var resultSubs = new List<SubscriptionNotifyInfo>();

            var diffSubscriptions = oldSubscriptions
               .Join(newSubscriptions,
                   oldSub => oldSub.Id,
                   newSub => newSub.Id,
                   (oldSub, newSub) => new { OldSubscription = oldSub, NewSubscription = newSub })
               .Where(sub => sub.NewSubscription.IsSubscription != sub.OldSubscription.IsSubscription)
               .Select(sub => sub.NewSubscription)
               .ToList();

            _logger.LogTrace("Список старых подписок {ls}", JsonConvert.SerializeObject(oldSubscriptions));
            _logger.LogTrace("Список новых подписок {ls}", JsonConvert.SerializeObject(newSubscriptions));
            _logger.LogTrace("Количество изменяемых подписок {cnt}", diffSubscriptions.Count);

            foreach (var subscription in diffSubscriptions)
            {
                try
                {
                    await _userService.ChangeRoleAsync(new ChangeRoleRequest
                    {
                        UserID = userId,
                        RoleID = subscription.Id,
                        Action = subscription.IsSubscription ? RoleAction.Set : RoleAction.Drop
                    }, cancellationToken);

                    resultSubs.Add(subscription);
                }
                catch (Exception ex) { _logger.LogTrace("Не удалось изменить подписки пользователя, работа продолжена. Ошибка:{ex}", ex.Message); }
            }

            resultSubs.AddRange(oldSubscriptions.Where(sub => !resultSubs.Any(existing => existing.Id == sub.Id)));

            return resultSubs;
        }
    }
}
