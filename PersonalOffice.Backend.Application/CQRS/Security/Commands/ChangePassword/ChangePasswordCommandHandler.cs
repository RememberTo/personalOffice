using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.User.Commands;
using PersonalOffice.Backend.Application.CQRS.User.Commands.ChangePassword;
using PersonalOffice.Backend.Application.CQRS.User.Queries.GetProfile;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Authenticate;
using PersonalOffice.Backend.Domain.Entities.User;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Security.Commands.ChangePassword
{
    internal class ChangePasswordCommandHandler(
        ILogger<GetUserProfileQueryHandler> logger,
        ITransportService transportService,
        IConfiguration configuration,
        INotificationService notificationService,
        IFileService fileService,
        IMailService mailService,
        IUserService userService) : IRequestHandler<ChangePasswordCommand, IResult>
    {
        private readonly ILogger<GetUserProfileQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IConfiguration _configuration = configuration;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IFileService _fileService = fileService;
        private readonly IMailService _mailService = mailService;
        private readonly IUserService _userService = userService;

        public async Task<IResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Аутентификация пользователя: {uid} для смены пароля", request.UserId);

            await AuthenticateUser(request.UserId, request.OldPasswordHash, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_SetPassword",
                Data = request,
            }, cancellationToken)).Data);

            if (!sqlResult.Success) throw new BadRequestException("Ошибка изменения пароля");

            foreach (var _ in Enumerable.Range(1, 15))
            {
                try
                {
                    await Task.Delay(75, cancellationToken);
                    var token = await AuthenticateUser(request.UserId, request.NewPasswordHash, cancellationToken);
                    await SendPassChangeNotification(request.UserId, cancellationToken);

                    return new Result(Domain.Common.Enums.InternalStatus.Success, "Пароль изменен", new AccessTokenVm { Token = token });
                }
                catch (Exception ex)
                {
                    _logger.LogError("{msg}", ex.Message);
                }
            }

            return new Result(Domain.Common.Enums.InternalStatus.Error, "Произошла ошибка при смене пароля");
        }

        private async Task<string> AuthenticateUser(int userId, string hashPassword, CancellationToken cancellationToken)
        {
            var authResult = Common.Global.Convert.DataTo<AuthResult>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Authenticator,
                Method = "AuthPO",
                Data = new AuthRequest
                {
                    Login = await _userService.GetUserLoginAsync(userId),
                    Password = hashPassword,
                    NoCache = true,
                    IsHash = true
                },
            }, cancellationToken)).Data);

            if (!authResult.IsAccessGranted) throw new UnauthorizedAccessException("Неверный пароль");

            return authResult.AuthToken;
        }

        private async Task SendPassChangeNotification(int userId, CancellationToken cancellationToken)
        {
            var sqlResultEmail = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<UserEmail>>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_GetUserEmail",
                Data = userId,
            }, cancellationToken)).Data);

            if (sqlResultEmail == null || !sqlResultEmail.Success || sqlResultEmail.ReturnValue?.Count() == 0)
            {
                _logger.LogError("Сообщение не будет отправлено, SQL ошибка: {msg}", sqlResultEmail?.Message);
                return;
            }

            var email = sqlResultEmail.ReturnValue?.First().Email;
            if (Env.Current != Env.Production || email is null)
                email = _configuration["Application:Mail:Default"]!;

            var msg = new Domain.Entities.Mail.MailMessage
            {
                To = [email],
                Application = "POBackend",
                Subject = "Пароль изменен",
                HtmlBody = await _fileService.GetResourcesFileContent("change-password-notify.html")
            };

            _mailService.TrySend(msg, out _);
        }
    }
}
