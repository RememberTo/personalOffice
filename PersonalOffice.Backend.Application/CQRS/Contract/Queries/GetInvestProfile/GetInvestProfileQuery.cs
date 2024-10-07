using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetInvestProfile
{
    /// <summary>
    /// Получение инвест профиля для договора
    /// </summary>
    public class GetInvestProfileQuery : IRequest<InvestProfileVm>
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
