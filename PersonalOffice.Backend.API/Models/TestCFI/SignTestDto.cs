using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Commands.SignTest;

namespace PersonalOffice.Backend.API.Models.TestCFI
{
    /// <summary>
    /// Контракт на обработку и подписание теста
    /// </summary>
    public class SignTestDto : IMapWith<SignTestCommand>
    {
        /// <summary>
        /// Код для подпсиания теста
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// Список ответов
        /// </summary>
        public required string Answers { get; set; }
        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile">Арфоиль маппинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SignTestDto, SignTestCommand>()
                .ForMember(x => x.Answers, opt => opt.MapFrom(src => ToEnumerable(src.Answers)));
        }

        private static IEnumerable<string> ToEnumerable(string str) => str.Split(";");
    }
}
