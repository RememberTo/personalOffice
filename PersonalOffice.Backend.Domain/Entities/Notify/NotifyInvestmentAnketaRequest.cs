using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Notify
{
    /// <summary>
    /// Контракт на добавление уведомления по инвестиционной анкете
    /// </summary>
    public class NotifyInvestmentAnketaRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.NotifyInvestmentAnketaRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("UserID")]
        public int UserId { get; set; }
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public required string FilePath { get; set; }
    }
}
