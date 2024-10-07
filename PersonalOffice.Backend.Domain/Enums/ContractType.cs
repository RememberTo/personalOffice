using System.Text.Json.Serialization;

namespace PersonalOffice.Backend.Domain.Enums
{
    /// <summary>
    /// Типы договоров
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContractType
    {
        /// <summary>
        /// Договор управления
        /// </summary>
        DU,
        /// <summary>
        /// Договор присоедениения
        /// </summary>
        DP,
        /// <summary>
        /// Договор присоедениения и Договор управления
        /// </summary>
        DUDP,
        /// <summary>
        /// Все договоры
        /// </summary>
        All
    }
}
