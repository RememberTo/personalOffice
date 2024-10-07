using MediatR;
using System.Text.Json.Serialization;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetMessagesFromTopic
{
    /// <summary>
    /// Контракт на получение сообщений из топика
    /// </summary>
    public class GetMessagesQuery : IRequest<IEnumerable<TopicMessageVm>>
    {
        /// <summary>
        /// Идентификатор топика
        /// </summary>
        public required int TopicId { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonIgnore]
        public required int UserId { get; set; }
    }
}
