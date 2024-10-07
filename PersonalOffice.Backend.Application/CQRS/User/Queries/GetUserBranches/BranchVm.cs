using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Domain.Entities.Contract;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserBranches
{
    /// <summary>
    /// Модель представления ветки пользователя
    /// </summary>
    public class BranchVm : IMapWith<BranchInfo>
    {
        /// <summary>
        /// Идентификатор ветки
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название ветки
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<BranchInfo, BranchVm>()
                .ForMember(x => x.Text, opt => opt.MapFrom(bi => bi.Name));
        }
    }
}
