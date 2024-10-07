using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Extesions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotifyById
{
    internal class NotifyMarkAsReadCommandHandler(
        ILogger<NotifyMarkAsReadCommandHandler> logger,
        ITransportService transportService
        ) : IRequestHandler<NotifyMarkAsReadCommand, NotificationVm>
    {
        private readonly ILogger<NotifyMarkAsReadCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<NotificationVm> Handle(NotifyMarkAsReadCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения уведомления: {nid} из микросервиса: {mName} метод: {mMethod}",
                request.NotifyId, MicroserviceNames.Notification, "PO_GetNotify");

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Notification,
                Method = "PO_GetNotify",
                Data = request
            }, cancellationToken);

            if (msg.Data.IsException(out Exception exception))
                throw new InvalidOperationException(exception.Message);
            if (msg.Data is null)
                throw new NotFoundException($"Уведомление {request.NotifyId} не найдено");

            _logger.LogTrace("Уведомление: {nid} из микросервиса: {mName} метод: {mMethod} получено",
               request.NotifyId, MicroserviceNames.Notification, "GetHistoryByUserID");

            var notify = Common.Global.Convert.DataTo<NotificationVm>(msg.Data);
            
            if (!notify.IsRead && request.MarkNotify)
                notify.IsRead = await MarkNotification(request, cancellationToken);

            return notify;
        }

        private async Task<bool> MarkNotification(NotifyMarkAsReadCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало пометки уведомления: {nid} как прочитанное в микросервис: {mName} метод: {mMethod} получено",
               request.NotifyId, MicroserviceNames.Notification, "MarkAsRead");

            var markRes = Common.Global.Convert.DataTo<MarkNotifyDto>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Notification,
                Method = "MarkAsRead",
                Data = request
            }, cancellationToken)).Data);

            if(markRes.Error is null)
            {
                _logger.LogTrace("Уведомление: {nid} прочитано", request.NotifyId);
                return true;
            }
            else
            {
                _logger.LogTrace("Уведомление: {nid} не прочитано ошибка: {ex}", request.NotifyId, markRes.Error.Message);
                return false;
            }         
        }
    }
}
