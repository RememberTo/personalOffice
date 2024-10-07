using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.File.Commands;

namespace PersonalOffice.Backend.API.Models.File
{
    /// <summary>
    /// Профиль мапинга
    /// </summary>
    public class FileMapped : IMapWith<IFormFile>
    {
        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile"></param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<IFormFile, UploadFile>()
                .ForMember(x => x.Length, up => up.MapFrom(form => form.Length))
                .ForMember(x => x.ContentType, up => up.MapFrom(form => form.ContentType))
                .ForMember(x => x.FileName, up => up.MapFrom(form => form.FileName))
                .ForMember(x => x.Stream, up => up.MapFrom(form => form.OpenReadStream()));
        }
    }
}
