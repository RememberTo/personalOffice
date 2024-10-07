using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Question.Commands.CreateTopic
{
    /// <summary>
    /// Контракт на создание Топика
    /// </summary>
    public class CreateTopicCommand : IRequest<IResult>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.QM_NewTopicRequest, MessageDataTypes";
        /// <summary>
        /// ID типа топика
        /// </summary>
        public int TopicTypeID { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Заголовок
        /// </summary>
        public required string Subject { get; set; }
        /// <summary>
        /// Первое сообщение в топике
        /// </summary>
        public required string Text { get; set; }
    }
}
