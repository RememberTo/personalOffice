using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateInvestProfileDocument;
using System.Collections.Generic;

namespace PersonalOffice.Backend.API.Models.Document
{
    /// <summary>
    /// Создание документа инвестиционного профиля
    /// </summary>
    public class CreateInvestProfileDocModel : IMapWith<CreateInvestProfileDocumentCommand>
    {
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public required int ContractId { get; set; }
        /// <summary>
        /// Параметры создания документа
        /// </summary>
        public required IDictionary<string, string> Params { get; set; }

        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile">Профиль мапинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateInvestProfileDocModel, CreateInvestProfileDocumentCommand>();
        }
    }
}
