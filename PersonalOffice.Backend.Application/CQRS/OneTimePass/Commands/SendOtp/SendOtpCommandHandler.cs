using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Question.Commands.SendMessage;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.OneTimePass;
using PersonalOffice.Backend.Domain.Entites.SMS;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.OneTimePass.Commands.SendOtp
{
    internal class SendOtpCommandHandler(
        ILogger<SendMessageCommandHandler> logger,
        ITransportService transportService,
        IUserService userService)
        : IRequestHandler<SendOtpCommand, IResult>
    {
        private readonly ILogger<SendMessageCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IUserService _userService = userService;

        public async Task<IResult> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Отправка запроса на получение номера телефона UserId {uId}", request.UserId);

            var phone = await _userService.GetPhoneByUserIdAsync(request.UserId, cancellationToken);

            _logger.LogInformation("Отправка запроса на формирование одноразового пароля для {p}", phone);

            var code = await GenerateOtpAsync(phone);
#pragma warning disable Удалить_на_продакшене
            //WARN!!! 
            if (Env.Current is Env.Development) 
            {
                _logger.LogWarning("ТОЛЬКО ДЛЯ ТЕСТОВОЙ СРЕДЫ Пароль {code} для {phone} отправлен в браузер", code, phone);
                return new Result(InternalStatus.Sent, message: code);
            }
            //
#pragma warning restore Удалить_на_продакшене
            _logger.LogInformation("Получен одноразовый пароль {code} для {phone}", code, phone);
            
            await SendSMSAsync(phone, code);

            return new Result(InternalStatus.Sent, string.Empty);

        }

        private async Task<string> GenerateOtpAsync(string phone)
        {
            var otpResult = Common.Global.Convert.DataTo<OtpOperationResult>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Otp,
                Method = "GenerateOtp",
                Data = new GenerateOtpCommand { ID = phone, TTL = 180 }
            }, TimeSpan.FromSeconds(5))).Data);

            if (otpResult.IsSuccess) return otpResult.Message;
            else throw new OneTimePassException(otpResult.Message);
        }
        private async Task<bool> SendSMSAsync(string phone, string code)
        {
            var smsResult = Common.Global.Convert.DataTo<SmsOperationResult>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.Sms,
                Method = "Send",
                Data = new SendSmsCommand { PhoneNumber = phone, IsFlash = false, Message = $"АО ИФК CОЛИД. Код для личного кабинета: {code}" }
            }, TimeSpan.FromSeconds(5))).Data);

            if (!smsResult.IsSuccess) throw new OneTimePassException($"Ошибка отправки СМС с одноразовым паролем: {smsResult.Message}");

            return true;
        }
    }
}
