using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.General;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractGraph
{
    internal class GetContractGraphQueryHandler(
        ILogger<GetContractGraphQueryHandler> logger,
        ITransportService transportService,
        IContractService contractService) : IRequestHandler<GetContractGraphQuery, IEnumerable<PointGraphVm>>
    {
        private readonly ILogger<GetContractGraphQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IContractService _contractService = contractService;

        public async Task<IEnumerable<PointGraphVm>> Handle(GetContractGraphQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Проверка на доступ к договору");
            await _contractService.CheckContract(request.ContractId, request.UserId, cancellationToken);

            request.GraphId = request.ContractId;

            _logger.LogTrace("Получение списка данных");
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetContractAmountGraph",
                Data = request,
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<PointGraphVm>>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success || sqlResult.ReturnValue is null)
            {
                _logger.LogTrace("Ошибка получения данных: {msg}", sqlResult?.Message);
                return [];
            }

            return sqlResult.ReturnValue;
        }
    }
}
