using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.Services.Base;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Contract;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using Convert = PersonalOffice.Backend.Application.Common.Global.Convert;

namespace PersonalOffice.Backend.Application.Services
{
    internal class ContractService(
        IService<ContractService> service,
        ICacheManager distributedCache) : IContractService
    {
        public readonly ILogger<ContractService> _logger = service.Logger;
        public readonly ITransportService _transportService = service.TransportService;
        private readonly ICacheManager _cache = distributedCache;

        public async Task CheckContract(int contractId, int userId, CancellationToken cancellationToken = default)
        {
            if((await GetContracts(userId, cancellationToken)).Any(c => c.Id == contractId) is false)
                throw new NotFoundException("Договор не найден");
        }

        public bool TryCheckContract(int contractId, int userId, out ContractInfo? contractInfo, CancellationToken cancellationToken = default)
        {
            try
            {
                var contracts = GetContracts(userId, cancellationToken).GetAwaiter().GetResult();
                contractInfo = contracts.First(contractInfo => contractInfo.Id == contractId);

                return true;
            }
            catch (Exception)
            {
                contractInfo = null;
                return false;
            }
        }

        public async Task<IEnumerable<ContractInfo>> GetContracts(int userId, CancellationToken cancellationToken = default)
        {
            var cacheKey = Convert.CachedKey("usercontracts", userId);
            var cachedContracts = await _cache.Get<IEnumerable<ContractInfo>>(cacheKey);

            if (cachedContracts != null)
                return cachedContracts;

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_UserContracts",
                Data = userId,
            }, cancellationToken);

            var sqlResult = Convert.DataTo<SQLOperationResult<IEnumerable<ContractInfo>>>(msg.Data);

            if(sqlResult is null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка получения данных SQL: {msg}", sqlResult?.Message);
                throw new InvalidOperationException("Ошибка получения данных");
            }

            await _cache.Set(cacheKey, sqlResult.ReturnValue ?? [], TimeSpan.FromMinutes(5));

            return sqlResult.ReturnValue ?? [];
        }

        public async Task<ContractInfo> GetContractById(int userId, int contractId, CancellationToken cancellationToken = default)
        {
            var contracts = await GetContracts(userId, cancellationToken);

            return contracts.FirstOrDefault(contractInfo => contractInfo.Id == contractId)
                ?? throw new NotFoundException("Договор не найден");
        }
    }
}
