using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContract;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Contract;
using PersonalOffice.Backend.Domain.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContractList
{
    internal class GetContractListQueryHandelr(
        ILogger<GetContractListQueryHandelr> logger,
        ITransportService transportService,
        IMapper mapper,
        IContractService contractService) : IRequestHandler<GetContractListQuery, IEnumerable<ContractVm>>
    {
        private readonly ILogger<GetContractListQueryHandelr> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IMapper _mapper = mapper;
        private readonly IContractService _contractService = contractService;

        public async Task<IEnumerable<ContractVm>> Handle(GetContractListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Получение договоров пользователя");
            var contractInfo = await _contractService.GetContracts(request.UserId, cancellationToken);

            _logger.LogTrace("Договоры получены");

            var contractsVm = _mapper.Map<IEnumerable<ContractVm>>(contractInfo
                .Where(x => x.IsActive 
                    && ((request.ContractType == ContractType.All)
                    || (request.ContractType == ContractType.DU && x.IsDu)
                    || (request.ContractType == ContractType.DP && x.IsDP)
                    || (request.ContractType == ContractType.DUDP && (x.IsDu || x.IsDP))))
                .OrderBy(x => x.DocNum));
               
            if (request.Currency is not null)
            {
                await AddContractAmount(contractsVm, request.Currency.Value, cancellationToken);
            }
           
            return contractsVm;
        }

        public async Task AddContractAmount(IEnumerable<ContractVm> contracts, Currency currency, CancellationToken cancellationToken)
        {
            foreach (var contract in contracts)
            {
                var contractData = await GetAmountAndProfiLossContract(contract.Id, currency, cancellationToken);
                contract.Amount = contractData.Amount;
                contract.ProfitLoss = contractData.ProfitLoss;
            }
        }

        private async Task<ContractDataResponse> GetAmountAndProfiLossContract(int contractId, Currency currency, CancellationToken cancellationToken)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetContractData",
                Data = new ContractDataRequest
                {
                    Id = contractId,
                    CurrencyId = (int)currency,
                    BeginDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today.AddDays(-1)
                },
            }, cancellationToken);

            try
            {
                var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<ContractDataResponse>>>(msg.Data);

                if (sqlResult == null || !sqlResult.Success || sqlResult.ReturnValue is null)
                {
                    _logger.LogTrace("Ошибка получения данных: {msg}", sqlResult?.Message);
                    return new ContractDataResponse();
                }

                return sqlResult.ReturnValue.FirstOrDefault() ?? new ContractDataResponse();
            }
            catch (Exception e)
            {
                _logger.LogTrace("Ошибка преобразования данных: {msg}", e.Message);
                return new ContractDataResponse();
            }
        }
    }
}
