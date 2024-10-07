using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.CQRS.Question.General;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopics
{
    /// <summary>
    /// Контракт на получение списка топиков
    /// </summary>
    public class GetTopicsQuery : IRequest<IEnumerable<TopicVm>>
    {
        [JsonProperty("$type")]
        private string _deserizlizeType => "MessageDataTypes.QM_TopicListRequest, MessageDataTypes";
        /// <summary>
        /// Тип топика ( 1- Чат с менеджером  2- Чат с инвестиционным консультантом) 
        /// </summary>
        public int TopicTypeID { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("ID")]
        public required int UserId { get; set; }
        /// <summary>
        /// Максимальное количество топиков в запросе
        /// </summary>
        public int MaxTopics { get; set; } = 100;
        /// <summary>
        /// Тип топиков
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// Страница
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Количество страниц
        /// </summary>
        public int PageSize { get; set; }


        /////// <summary>
        /////// Мапинг данных
        /////// </summary>
        /////// <param name="profile">Профиль типа</param>
        ////public void Mapping(Profile profile)
        ////{
        ////    profile.CreateMap<(int userID, string TopicTypeCode), GetTopicsQuery>()
        ////        .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.userID))
        ////        .ForMember(x => x.TopicTypeID, opt => opt.MapFrom(src => ToInt(src.TopicTypeCode)));
        ////}

        /// <summary>
        /// Преобразование строкового кода темы чата в целое число
        /// </summary>
        /// <param name="TopicTypeCode">строковый код типа темы чата</param>
        /// <returns></returns>
        public int TopicTypeCodeToInt(string? TopicTypeCode)
        {
            if (TopicTypeCode == null)
                return (int)QuestionTopicType.ChatWithManager;
            else
                return (int)(QuestionTopicType)Enum.Parse(typeof(QuestionTopicType), TopicTypeCode);
        }

    }
}

