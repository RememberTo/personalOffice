using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById.Vm;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById
{
    /// <summary>
    /// Контракт на получение документа
    /// </summary>
    public class GetDocByIdQuery : IRequest<DocumentBaseVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_InfoRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентфиикатор документа
        /// </summary>
        public int DocId { get; set; }
    }
}
