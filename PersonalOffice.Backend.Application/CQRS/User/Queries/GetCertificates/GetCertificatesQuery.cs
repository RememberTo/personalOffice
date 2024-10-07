using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetCertificates
{
    /// <summary>
    /// Контракт на получение списка сертификатов
    /// </summary>
    public class GetCertificatesQuery : IRequest<IEnumerable<CertificateVm>>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
    }
}
