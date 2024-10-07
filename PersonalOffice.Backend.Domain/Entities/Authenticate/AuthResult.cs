using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Authenticate
{
    /// <summary>
    /// Результат аутентификации
    /// </summary>
    public class AuthResult
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.LoginPassAuthRequest, MessageDataTypes";
        /// <summary>
        /// Доступ разрешен
        /// </summary>
        public bool IsAccessGranted {  get; set; }
        /// <summary>
        /// Логин заблокирован
        /// </summary>
        public bool IsLoginDisabled { get; set; }
        /// <summary>
        /// Токен аутентификации
        /// </summary>
        public required string AuthToken { get; set; }
    }
}
