using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Commands.SignTest
{
    /// <summary>
    /// Контракт на верификацию одноразового кода
    /// </summary>
    public class CodeVerificationCommand
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.CheckOtpRequest, MessageDataTypes";
        /// <summary>
        /// Номер телефона
        /// </summary>
        public required string ID { get; set; }
        /// <summary>
        /// Код верификации
        /// </summary>
        public required string Otp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool MarkUsed { get; set; } = true;
    }
}
