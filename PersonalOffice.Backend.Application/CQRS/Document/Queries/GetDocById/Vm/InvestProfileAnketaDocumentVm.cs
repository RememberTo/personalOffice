using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Domain.Entities.Document.Info;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById.Vm
{
    /// <summary>
    /// Модель представления документа инвестиционного профиля 
    /// </summary>
    public class InvestProfileAnketaDocumentVm : DocumentBaseVm, IMapWith<InvestProfileAnketaInfo>
    {
        /// <summary>
        /// Элементы инвестиционного профиля, различные определяющие инвест профиль 
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Elements { get; set; } = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InvestProfileAnketaInfo, InvestProfileAnketaDocumentVm>()
                .ForMember(x => x.Elements, opt => opt.MapFrom(y => Common.Global.Convert.MapKeyValues(y.Elements)));
        }
    }
}
