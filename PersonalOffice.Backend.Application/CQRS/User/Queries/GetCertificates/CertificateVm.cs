using AutoMapper;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Domain.Entites.User;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetCertificates
{
    /// <summary>
    /// Модель представления сертификата
    /// </summary>
    public class CertificateVm : IMapWith<UserCertificateInfo>
    {
        /// <summary>
        /// Отпечаток
        /// </summary>
        public required string Thumbprint { get; set; }
        /// <summary>
        /// Кому выдан сертификат
        /// </summary>
        public required string Subject { get; set; }
        /// <summary>
        /// Эмитент
        /// </summary>
        public required string Issuer { get; set; }
        /// <summary>
        /// Действителен с 
        /// </summary>
        public DateTime NotBefore { get; set; }
        /// <summary>
        /// Действителен по
        /// </summary>
        public DateTime NotAfter { get; set; }

        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile">профиль мапинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserCertificateInfo, CertificateVm>()
                .ForMember(cv => cv.Thumbprint, opt => opt.MapFrom(uc => uc.Certificate.Thumbprint))
                .ForMember(cv => cv.Issuer, opt => opt.MapFrom(uc => Format.GetCNFromName(uc.Certificate.Issuer)))
                .ForMember(cv => cv.Subject, opt => opt.MapFrom(uc => Format.GetCNFromName(uc.Certificate.Subject)))
                .ForMember(cv => cv.NotBefore, opt => opt.MapFrom(uc => uc.Certificate.NotBefore))
                .ForMember(cv => cv.NotAfter, opt => opt.MapFrom(uc => uc.Certificate.NotAfter));
        }
    }
}
