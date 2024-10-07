using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.SetStatusDocument
{
    /// <summary>
    /// Контракт на изменение статуса документа
    /// </summary>
    public class SetStatusDocumentCommand : IRequest<IResult>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_StatusRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("UserID")]
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        [JsonProperty("DocID")]
        public int DocumentId { get; set; }
        /// <summary>
        /// Статус
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Комментарий к статусу
        /// </summary>
        public string? StatusComment { get; set; }
    }
}
