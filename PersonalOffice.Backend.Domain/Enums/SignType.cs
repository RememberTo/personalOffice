using System.Text.Json.Serialization;

namespace PersonalOffice.Backend.Domain.Enums
{
    /// <summary>
    /// Типы подписи
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public enum SignType
    {
        /// <summary>
        /// Подпись с помощью смс кода
        /// </summary>
        SmsCode,
        /// <summary>
        /// Подпись с помощью сертификата
        /// </summary>
        Eds,
    }
}
