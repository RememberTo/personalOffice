using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.Services.Base;
using PersonalOffice.Backend.Domain.Entites.OneTimePass;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.OneTimePass;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.Services
{
    internal class OneTimePassService(IService<OneTimePassService> service) : IOneTimePassService
    {
        private readonly ILogger<OneTimePassService> _logger = service.Logger;
        private readonly ITransportService _transportService = service.TransportService;

        public async Task CheckOtpAsync(CheckOtp command, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Проверка одноразового пароля для пользователя: номер телефона: {ph}, код; {cd}", command.Id, command.Otp);

            var otpResult = Common.Global.Convert.DataTo<OtpOperationResult>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Otp,
                Method = "CheckOtp",
                Data = command 
            }, TimeSpan.FromSeconds(5))).Data);

            if (!otpResult.IsSuccess)
            {
                _logger.LogError("Ошибка проверки одноразового пароля: {msg}", otpResult.Message);
                throw new OneTimePassException(otpResult.Message);
            }

            _logger.LogTrace("Успешная проверка одноразового пароля для пользователя: номер телефона: {ph}, код: {cd}", command.Id, command.Otp);
        }
    }
}
