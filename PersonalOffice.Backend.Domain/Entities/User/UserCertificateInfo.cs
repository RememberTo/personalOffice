using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace PersonalOffice.Backend.Domain.Entites.User
{
    /// <summary>
    /// Информация о сертификате пользователя
    /// </summary>
    public class UserCertificateInfo
    {
        /// <summary>
        /// Идентфиикатор сертификата
        /// </summary>
        [JsonProperty("ID")]
        public int Id {  get; set; }
        /// <summary>
        /// Сертификат
        /// </summary>
        public required X509Certificate2 Certificate {  get; set; }
    }
}
