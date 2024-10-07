using MediatR;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContract;
using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContractList
{
    /// <summary>
    /// Контракт на получение договоров
    /// </summary>
    public class GetContractListQuery : IRequest<IEnumerable<ContractVm>>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Тип договора
        /// </summary>
        public ContractType ContractType { get; set; } = ContractType.All;
        /// <summary>
        /// Валюта в договоре (необязательно)
        /// </summary>
        public Currency? Currency { get; set; }
    }
}
