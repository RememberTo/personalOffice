using AutoMapper;
using PersonalOffice.Backend.Application.Common.Mappings;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.SignDocument;
using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.API.Models.Document
{
    /// <summary>
    /// Контракт для подписания документа
    /// </summary>
    public class SignDocumentDto : IMapWith<SignDocumentCommand>
    {
        /// <summary>
        /// Тип подписания документа
        /// </summary>
        public SignType SignType { get; set; }
        /// <summary>
        /// Смс код подписания документа
        /// </summary>
        public string? SmsCode { get; set; }
        /// <summary>
        /// Хэш сертификата
        /// </summary>
        public string? HashCertificate {  get; set; }
        /// <summary>
        /// Маппинг
        /// </summary>
        /// <param name="profile">Профиль мапинга</param>
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SignDocumentDto, SignDocumentCommand>();
        }
    }
}