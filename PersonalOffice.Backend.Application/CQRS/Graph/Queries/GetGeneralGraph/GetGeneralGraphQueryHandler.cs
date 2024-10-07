using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.General;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Numerics;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetGeneralGraph
{
    internal class GetGeneralGraphQueryHandler(
        ILogger<GetGeneralGraphQueryHandler> logger,
        ITransportService transportService) : IRequestHandler<GetGeneralGraphQuery, IEnumerable<PointGraphVm>>
    {
        private readonly ILogger<GetGeneralGraphQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<IEnumerable<PointGraphVm>> Handle(GetGeneralGraphQuery request, CancellationToken cancellationToken)
        {
            request.GraphId = request.UserId;

            _logger.LogTrace("Получение списка данных");
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "SF_GetGeneralGraph",
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
