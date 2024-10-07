using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.Domain.Entities.User
{
    /// <summary>
    /// Контракт на изменение роли
    /// </summary>
    public class ChangeRoleRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.PO_ChangeRoleRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// Действие
        /// </summary>
        public RoleAction Action { get; set; }
    }
}
