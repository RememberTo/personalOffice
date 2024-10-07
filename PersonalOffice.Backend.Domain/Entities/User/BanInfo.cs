using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.User
{
    /// <summary>
    /// Информация о блокировке
    /// </summary>
    public class BanInfo
    {
        /// <summary>
        /// Идентификатор типа блокировки
        /// </summary>
        [JsonProperty("TypeID")]
        public int TypeId { get; set; }
        /// <summary>
        /// Наименование причины
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Reason { get; set; }
        /// <summary>
        /// Начальная дата блокировки
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// Дата снятия блокировки
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
