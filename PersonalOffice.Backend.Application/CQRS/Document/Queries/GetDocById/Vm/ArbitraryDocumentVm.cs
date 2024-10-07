using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Domain.Entities.Document.Info;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById.Vm
{
    /// <summary>
    /// Модель представления произвольного документа
    /// </summary>
    public class ArbitraryDocumentVm : DocumentBaseVm, IMapWith<DocumentArbitraryInfo>
    {
        /// <summary>
        /// Название
        /// </summary>
        public required string DocName { get; set; }
        /// <summary>
        /// Коментарий
        /// </summary>
        public required string DocComment { get; set; }
        /// <summary>
        /// Хэш прикрепленных файлов
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> FilesHash { get; set; } = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DocumentArbitraryInfo, ArbitraryDocumentVm>()
                .ForMember(x => x.FilesHash, opt => opt.MapFrom(y => Common.Global.Convert.MapKeyValues(y.FilesHash)))
                .ForMember(x => x.DocComment, opt => opt.MapFrom(y => y.DocComment))
                .ForMember(x => x.DocName, opt => opt.MapFrom(y => y.DocName));
        }

        
    }
}
