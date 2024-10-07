using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.Question.Commands.CreateTopic;
using PersonalOffice.Backend.Application.CQRS.Question.General;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PersonalOffice.Backend.API.Models.Question
{
    /// <summary>
    /// Модель для создания нового топика
    /// </summary>
    public class NewTopicDto : IMapWith<CreateTopicCommand>
    {
        /// <summary>
        /// ID типа топика
        /// </summary>
        public string? TopicTypeCode { get; set; }
        /// <summary>
        /// Заголовок топика
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [NotNull]
        public required string Subject { get; set; }
        /// <summary>
        /// Текст сообщения
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [NotNull]
        public required string Text { get; set; }

        /// <summary>
        /// Мапинг данных
        /// </summary>
        /// <param name="profile">Профиль типа</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewTopicDto, CreateTopicCommand>()
                .ForMember(x => x.TopicTypeID, opt => opt.MapFrom(src => ToInt(src.TopicTypeCode))).ReverseMap();
        }

        private static int ToInt(string? TopicTypeCode)
        {
            if (TopicTypeCode == null)
                return (int)QuestionTopicType.ChatWithManager;    
            else
                return (int)(QuestionTopicType)Enum.Parse(typeof(QuestionTopicType), TopicTypeCode);
        }
    }
}
