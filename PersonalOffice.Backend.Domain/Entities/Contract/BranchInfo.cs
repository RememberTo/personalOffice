using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Contract
{
    /// <summary>
    /// Информация о ветке пользователя
    /// </summary>
    public class BranchInfo
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonProperty("BranchID")]
        public int Id { get; set; }
        /// <summary>
        /// Название ветки
        /// </summary>
        [JsonProperty("BranchName")]
        public string Name { get; set; } = string.Empty;
    }
}
