using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.User
{
    /// <summary>
    /// Информация о роле пользователя
    /// </summary>
    public class RoleInfo
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        [JsonProperty("ID")]
        public int Id { get; set; }
        /// <summary>
        /// Наименование роли
        /// </summary>
        public required string Name { get; set; }
    }
}
