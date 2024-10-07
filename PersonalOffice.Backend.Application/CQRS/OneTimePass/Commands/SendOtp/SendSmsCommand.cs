using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.OneTimePass.Commands.SendOtp
{
    /// <summary>
    /// DTO на отправку смс
    /// </summary>
    public class SendSmsCommand
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.SendSmsRequest, MessageDataTypes";
        /// <summary>
        /// Номер телефона куда отправлять СМС
        /// </summary>
        public required string PhoneNumber { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public required string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsFlash { get; set; }
    }
}
