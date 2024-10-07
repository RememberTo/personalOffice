using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetFilesById
{
    internal class GetFilesQueryHandler(
        ILogger<GetFilesQueryHandler> logger,
        IUserService userService,
        ITransportService transportService) : IRequestHandler<GetFilesQuery, IEnumerable<NotifyFileVm>>
    {
        private readonly ILogger<GetFilesQueryHandler> _logger = logger;
        private readonly IUserService _userService = userService;
        private readonly ITransportService _transportService = transportService;

        public async Task<IEnumerable<NotifyFileVm>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
        {
            await _userService.CheckUserNotifyFilesAccessAsync(request.UserId ,request.NotifyId, cancellationToken);

            _logger.LogTrace("Начало получения списка файлов для уведомления {nid} из микросервиса: {mName} метод: {mMethod}",
                request.NotifyId, MicroserviceNames.Notification, "GetHistoryByUserID");

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Notification,
                Method = "GetFiles4Notify",
                Data = request.NotifyId
            }, cancellationToken);

            var notifyfilesVm = Common.Global.Convert.DataTo<IEnumerable<NotifyFileVm>>(msg.Data);

            _logger.LogTrace("{cnt} файлов получено для уведомления {nid} из микросервиса: {mName} метод: {mMethod}",
                notifyfilesVm.Count(), request.NotifyId, MicroserviceNames.Notification, "GetHistoryByUserID");

            return notifyfilesVm;
        }
    }
}
