using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetCustomReport
{
    internal class GetFileListQuery
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.FilesMinIORequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId {  get; set; }
        /// <summary>
        /// Наименование бакета в minIO
        /// </summary>
        public string? Bucket {  get; set; }
    }
}
