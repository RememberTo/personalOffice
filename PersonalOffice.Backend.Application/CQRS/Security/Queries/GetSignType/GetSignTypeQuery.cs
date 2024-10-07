using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.Security.Queries.GetSignType
{
    /// <summary>
    /// Контракт на получение типа подписи
    /// </summary>
    public class GetSignTypeQuery : IRequest<SignTypeVm>
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
    }
}
