using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entites.Pagination
{
    /// <summary>
    /// Параметры пагинации
    /// </summary>
    public class PageConfiguration
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.PageConfiguration, MessageDataTypes";
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber { get; set; } = 0;
        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        public int PageSize { get; set; } = 0;
    }
}
