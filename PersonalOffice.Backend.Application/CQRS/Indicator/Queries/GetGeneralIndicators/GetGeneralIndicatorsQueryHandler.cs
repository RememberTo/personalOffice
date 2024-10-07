using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetContractIndicators;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Order;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetGeneralIndicators
{
    internal class GetGeneralIndicatorsQueryHandler(
        ILogger<GetGeneralIndicatorsQueryHandler> logger,
        ITransportService transportService) : IRequestHandler<GetGeneralIndicatorsQuery, IndicatorsVm>
    {
        private readonly ILogger<GetGeneralIndicatorsQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<IndicatorsVm> Handle(GetGeneralIndicatorsQuery request, CancellationToken cancellationToken)
        {
            request.GraphId = request.UserId;

            _logger.LogTrace("Получение списка данных");
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetGeneralIndicators",
                Data = request,
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<Domain.Entities.Order.Indicator>>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success || sqlResult.ReturnValue is null)
            {
                _logger.LogTrace("Ошибка получения данных: {msg}", sqlResult?.Message);
                throw new InvalidOperationException("Не удалось получить индикаторы");
            }

            if (!sqlResult.ReturnValue.Any())
            {
                return new IndicatorsVm();
            }

            return new IndicatorsVm
            {
                Free = sqlResult.ReturnValue.First(x => x.Code == "investment_portfolio_info_AmountFree").Amount,
                ProfitLoss = sqlResult.ReturnValue.First(x => x.Code == "investment_portfolio_info_ProfitLoss").Amount,
                Security = sqlResult.ReturnValue.First(x => x.Code == "investment_portfolio_info_AmountSecurity").Amount,
                Total = sqlResult.ReturnValue.First(x => x.Code == "investment_portfolio_info_AmountEnd").Amount,
            };
        }
    }
}
