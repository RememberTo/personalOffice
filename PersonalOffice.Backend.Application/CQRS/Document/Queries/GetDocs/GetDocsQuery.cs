using MediatR;
using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocs
{
    /// <summary>
    /// Контракт на предоставление спсика документов
    /// </summary>
    public class GetDocsQuery : IRequest<DocumentsInfoVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_ClientDocListRequest, MessageDataTypes";
        /// <summary>
        /// Идентфиикатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page {  get; set; }
        /// <summary>
        /// Вместимость 1 страницы
        /// </summary>
        public int PageSize { get; set; }
    }
}
