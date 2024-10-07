using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateArbitraryDocument;

namespace PersonalOffice.Backend.API.Models.Document
{
    /// <summary>
    /// Модель формы создания произвольного документа
    /// </summary>
    public class CreateArbitraryDocModel : IMapWith<CreateArbitraryDocumentCommand>
    {
        /// <summary>
        /// Название документа
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Комментарий к документу
        /// </summary>
        public string? Comment { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public required int ContractId { get; set; }

        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile">Профиль мапинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateArbitraryDocModel, CreateArbitraryDocumentCommand>();
        }
    }
}