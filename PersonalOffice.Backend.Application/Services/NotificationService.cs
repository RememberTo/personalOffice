using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.Services.Base;
using PersonalOffice.Backend.Domain.Entites.Notify;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.Services
{
    internal class NotificationService(IService<NotificationService> service) : INotificationService
    {
        private readonly ITransportService _transportService = service.TransportService;
        private readonly ILogger<NotificationService> _logger = service.Logger;

        public async Task SendNotificationMessageAsync(NotificationParameters parameters)
        {
            _logger.LogTrace("Начало отправки уведомления пользователю: {uid} сообщение: {msg}", parameters.UserID, parameters.Message);

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_SendNotificationMessage",
                Data = parameters
            }, TimeSpan.FromSeconds(10));

            var SQLResult = Common.Global.Convert.DataTo<SQLOperationResult<string>>(msg.Data);

            if (!SQLResult.Success) { throw new InvalidOperationException(SQLResult.Message); }

            _logger.LogTrace("Уведомление пользователю: {uid} отправлено", parameters.UserID);
        }
    }
}
