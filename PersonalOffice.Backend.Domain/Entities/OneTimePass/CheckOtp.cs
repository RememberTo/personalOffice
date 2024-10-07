using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.OneTimePass
{
    /// <summary>
    /// Контракт на проверку одноразового пароля
    /// </summary>
    public class CheckOtp
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.CheckOtpRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор (телефон пользователя)
        /// </summary>
        [JsonProperty("ID")]
        public required string Id { get; set; }
        /// <summary>
        /// Одноразовый пароль (код)
        /// </summary>
        public required string Otp {  get; set; }
        /// <summary>
        /// Помечать что код уже использован по умолчанию true
        /// </summary>
        public bool MarkUsed { get; set; } = true;
    }
}
