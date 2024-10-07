using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entites.SQL
{
    /// <summary>
    /// Контракт для запроса информации по уникальным идентификаторам
    /// </summary>
    public class IdRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.IdRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int UniqueId { get; set; }
    }
}
