using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Question.Commands.SendMessage
{
    /// <summary>
    /// Контракт на отрпавку сообщения
    /// </summary>
    public class SendMessageCommand : IRequest<IResult>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.QM_UserSendMessageRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonIgnore]
        public required int UserId { get; set; }
        /// <summary>
        /// Идентификатор топика
        /// </summary>
        public required int TopicId { get; set; }
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public required string Text { get; set; }
    }
}