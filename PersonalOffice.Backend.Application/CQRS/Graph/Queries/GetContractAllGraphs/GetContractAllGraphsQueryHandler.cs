using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.General;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Graph;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractAllGraphs
{
    internal class GetContractAllGraphsQueryHandler(
        ILogger<GetContractAllGraphsQueryHandler> logger,
        IMapper mapper,
        IDistributedCache cache,
        IContractService contractService,
        ITransportService transportService) : IRequestHandler<GetContractAllGraphsQuery, IEnumerable<AllGraphVm>>
    {
        private readonly ILogger<GetContractAllGraphsQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IDistributedCache _cache = cache;
        private readonly IContractService _contractService = contractService;
        private readonly ITransportService _transportService = transportService;

        public async Task<IEnumerable<AllGraphVm>> Handle(GetContractAllGraphsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Проверка на доступ к договору");
            await _contractService.CheckContract(request.ContractId, request.UserId, cancellationToken);

            request.GraphId = request.ContractId;

            _logger.LogTrace("Получение списка данных");
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetAllContractGraphs",
                Data = request,
            }, TimeSpan.FromMinutes(12));

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<AllGraphResult>>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success || sqlResult.ReturnValue is null)
            {
                _logger.LogTrace("Ошибка получения данных: {msg}", sqlResult?.Message);
                return [];
            }

            return _mapper.Map<IEnumerable<AllGraphVm>>(sqlResult.ReturnValue);
        }
    }
}
