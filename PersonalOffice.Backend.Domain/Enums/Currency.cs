using System.Text.Json.Serialization;

namespace PersonalOffice.Backend.Domain.Enums
{
    /// <summary>
    /// Валюты
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        /// <summary>
        /// Рубль РФ
        /// </summary>
        RoubleRF = 1,
        /// <summary>
        /// Доллар США
        /// </summary>
        DollarUSA = 2028,
        /// <summary>
        /// Евро
        /// </summary>
        Euro = 4568,
        /// <summary>
        /// Китайский юань
        /// </summary>
        YuanChina = 28761
    }
}
