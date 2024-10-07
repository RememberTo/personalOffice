using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.Services.Base;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Mail;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using Convert = PersonalOffice.Backend.Application.Common.Global.Convert;

namespace PersonalOffice.Backend.Application.Services
{
    internal class MailService(IService<MailService> service) : IMailService
    {
        private readonly ILogger<MailService> _logger = service.Logger;
        private readonly ITransportService _transportService = service.TransportService;


        public async Task SendAsync(MailMessage message, CancellationToken cancellationToken = default)
        {
            var request = new MailRequest { RequestID = Guid.NewGuid().ToString(), Message = message };
            _logger.LogTrace("Отправка сообщения requestId {rid}", request.RequestID);

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.MailSender,
                Method = "SendAsync",
                Data = request
            }, cancellationToken);

            var result = Convert.DataTo<MailResponse>(msg.Data);

            _logger.LogTrace("Сообщение: {id}, добавлено и ждет отправки requestId {rid}", result.MailID, result.RequestID);
        }

        public bool TrySend(MailMessage message, out MailResponse? mailResponse)
        {
            try
            {
                var request = new MailRequest { RequestID = Guid.NewGuid().ToString(), Message = message };
                _logger.LogTrace("Отправка сообщения requestId {rid}", request.RequestID);

                var msg = _transportService.RPCServiceAsync(new Message
                {
                    Source = MicroserviceNames.Backend,
                    Destination = MicroserviceNames.MailSender,
                    Method = "SendAsync",
                    Data = request
                }, TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();

                mailResponse = Convert.DataTo<MailResponse>(msg.Data);
                _logger.LogTrace("Сообщение: {id}, добавлено и ждет отправки requestId {rid}", mailResponse.MailID, mailResponse.RequestID);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка: {msg}", ex.Message);
                
                mailResponse = null;
                return false;
            }
        }
    }
}
