using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify
{
    internal class GetNotifyQueryHandler(
        ILogger<GetNotifyQueryHandler> logger,
        ITransportService transportService
        ) : IRequestHandler<GetNotifyQuery, NotificationsVm>
    {
        private readonly ILogger<GetNotifyQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<NotificationsVm> Handle(GetNotifyQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения {cnt} уведомлений из микросервиса: {mName} метод: {mMethod}",
                request.PageConfig.PageSize, MicroserviceNames.Notification, "GetHistoryByUserID");

            var count = request.PageConfig.PageSize == 0 ? int.MaxValue : request.PageConfig.PageSize;
            request.PageConfig.PageSize = int.MaxValue;

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Notification,
                Method = "GetHistoryByUserID",
                Data = request
            }, cancellationToken);

            var historyDto = Common.Global.Convert.DataTo<HistoryDto>(msg.Data);

            _logger.LogTrace("{cnt} Уведомлений получено из микросервиса: {mName} метод: {mMethod}",
                historyDto.History.Count(), MicroserviceNames.Notification, "GetHistoryByUserID");

            return new NotificationsVm 
            { 
                Notifications = historyDto.History.Take(count), 
                UnreadCount = historyDto.History.Where(h => h.IsRead == false).Count() 
            };
        }
    }
}
