using MediatR;
using PersonalOffice.Backend.Domain.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.SignDocument
{
    /// <summary>
    /// Контракт на подписание документа
    /// </summary>
    public class SignDocumentCommand : IRequest<IResult>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public int DocumentId { get; set; }
        /// <summary>
        /// Тип подписания документа
        /// </summary>
        public SignType SignType { get; set; }
        /// <summary>
        /// Смс код подписания документа
        /// </summary>
        public string? SmsCode
        {
            get => this.SignType == SignType.SmsCode ? _smsCode : null;
            set => _smsCode = value;
        }
        private string? _smsCode;

        /// <summary>
        /// Хэш сертификата
        /// </summary>
        public string? HashCertificate
        {
            get => this.SignType == SignType.Eds ? _hashCertificate : null;
            set => _hashCertificate = value;
        }
        private string? _hashCertificate;
    }
}
