using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.Report
{
    /// <summary>
    /// Контаркт байтового ответа
    /// </summary>
    public class ObjectBytes
    {
        /// <summary>
        /// Строка байт в формате 64
        /// </summary>
        [JsonProperty("$value")]
        public required string Value { get; set; }

    }
}
