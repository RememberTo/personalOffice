using MediatR;
using System.Text.Json.Serialization;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopicById
{
    /// <summary>
    /// Контракт на получение информации о топика
    /// </summary>
    public class GetTopicByIdQuery : IRequest<TopicInfoVm>
    {
        /// <summary>
        /// Идентфиикатор топика
        /// </summary>
        public required int TopicId { get; set; }

        /// <summary>
        /// Иденотификатор пользователя
        /// </summary>
        [JsonIgnore]
        public required int UserId { get; set; }
    }
}
