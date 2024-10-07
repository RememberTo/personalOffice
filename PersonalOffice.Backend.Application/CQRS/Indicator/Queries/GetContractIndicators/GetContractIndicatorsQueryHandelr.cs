using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractGraph;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetContractIndicators
{
    internal class GetContractIndicatorsQueryHandelr(
        ILogger<GetContractGraphQueryHandler> logger,
        IContractService contractService,
        ITransportService transportService) : IRequestHandler<GetContractIndicatorsQuery, IndicatorsVm>
    {
        private readonly ILogger<GetContractGraphQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IContractService _contractService = contractService;
        public async Task<IndicatorsVm> Handle(GetContractIndicatorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Проверка на доступ к договору");
            var contract = await _contractService.GetContractById(request.UserId, request.ContractId, cancellationToken);

            request.GraphId = request.ContractId;

            _logger.LogTrace("Получение списка данных");
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetContractIndicators",
                Data = request,
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<Domain.Entities.Order.Indicator>>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success || sqlResult.ReturnValue is null)
            {
                _logger.LogTrace("Ошибка получения данных: {msg}", sqlResult?.Message);
                throw new InvalidOperationException("Не удалось получить индикаторы");
            }

            return new IndicatorsVm
            {
                Free = sqlResult.ReturnValue.First(x => x.Code == "investment_portfolio_info_AmountFree").Amount,
                ProfitLoss = sqlResult.ReturnValue.First(x => x.Code == "investment_portfolio_info_ProfitLoss").Amount,
                Security = sqlResult.ReturnValue.First(x => x.Code == "investment_portfolio_info_AmountSecurity").Amount,
                Total = sqlResult.ReturnValue.First(x => x.Code == "investment_portfolio_info_AmountEnd").Amount,
                IsActive = contract.IsActive,
                DocNum = contract.DocNum
            };
        }
    }
}
