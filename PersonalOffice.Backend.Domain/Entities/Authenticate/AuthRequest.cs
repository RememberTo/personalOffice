using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Authenticate
{
    /// <summary>
    /// Контракт на аутентификацию
    /// </summary>
    public class AuthRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.LoginPassAuthRequest, MessageDataTypes";
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public required string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public required string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Domain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool NoCache { get; set; }
        /// <summary>
        /// Пароль в хешированном состоянии
        /// </summary>
        public bool IsHash { get; set; } = false;
    }
}
