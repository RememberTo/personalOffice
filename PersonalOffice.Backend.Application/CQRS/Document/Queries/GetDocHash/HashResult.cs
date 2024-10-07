using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocHash
{
    internal class HashResult
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.HashResult, MessageDataTypes";
        public bool Success { get; set; }
        public string? Hash {  get; set; }
        public string? ErrorMessage { get; set; }
    }
}
