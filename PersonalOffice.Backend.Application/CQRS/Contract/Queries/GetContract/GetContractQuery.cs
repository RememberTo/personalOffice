using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContract
{
    /// <summary>
    /// Контракт на получение информации о договоре
    /// </summary>
    public class GetContractQuery : IRequest<ContractVm>
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
