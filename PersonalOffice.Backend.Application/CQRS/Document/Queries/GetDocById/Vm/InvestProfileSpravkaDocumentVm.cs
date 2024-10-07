using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using PersonalOffice.Backend.Domain.Entities.Document.Info;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById.Vm
{
    /// <summary>
    /// Модель представления инвест справок
    /// </summary>
    public class InvestProfileSpravkaDocumentVm : DocumentBaseVm, IMapWith<InvestProfileSparvkaInfo>
    {
        /// <summary>
        /// Файлы инвестиционных српавок
        /// </summary>
        public IEnumerable<DocumentFileInfo> Files { get; set; } = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<InvestProfileSparvkaInfo, InvestProfileSpravkaDocumentVm>();
        }
    }
}
