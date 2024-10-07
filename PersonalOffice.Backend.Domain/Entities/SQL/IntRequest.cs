using Newtonsoft.Json;

namespace PersonalOffice.Backend.Domain.Entities.SQL
{
    /// <summary>
    /// Контракт на передачу целочисленного значения
    /// </summary>
    public class IntRequest
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.IntParamRequest, MessageDataTypes";
        /// <summary>
        /// Целочисленное значение
        /// </summary>
        public int Value { get; set; }
    }
}
