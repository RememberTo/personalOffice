using PersonalOffice.Backend.Domain.Entities.Contract;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с догвоорами
    /// </summary>
    public interface IContractService
    {
        /// <summary>
        /// Проверка на доступ к договору
        /// </summary>
        /// <param name="contractId">идентификатор контракта</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task CheckContract(int contractId, int userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Проверка на доступ к договору по схеме TryXXX
        /// </summary>
        /// <param name="contractId">идентификатор контракта</param>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="contractInfo">out параметр договора</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public bool TryCheckContract(int contractId, int userId, out ContractInfo? contractInfo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение всех договоров пользователя
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<IEnumerable<ContractInfo>> GetContracts(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение всех договоров пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="contractId">Идентфиикатор договора</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<ContractInfo> GetContractById(int userId, int contractId, CancellationToken cancellationToken = default);
    }
}
