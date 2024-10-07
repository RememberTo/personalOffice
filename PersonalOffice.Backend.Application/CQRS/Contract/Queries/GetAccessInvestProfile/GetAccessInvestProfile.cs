using MediatR;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetAccessInvestProfile
{
    /// <summary>
    /// Контракт на получение статсуса, по подаче заявление на инвестиционный профиль
    /// </summary>
    public class GetAccessInvestProfile : IRequest<IResult>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int ContractId { get; set; }
    }
}
