using System.Text.Json.Serialization;

namespace PersonalOffice.Backend.Domain.Enums
{
    /// <summary>
    /// Варианты пополнения
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentOptions
    {
        /// <summary>
        /// С использованием СБП
        /// </summary>
        SBP = 0,
        /// <summary>
        /// По QR-коду
        /// </summary>
        QRCode = 1,
        /// <summary>
        /// По реквизитам
        /// </summary>
        BankDetails = 2
    }
}
