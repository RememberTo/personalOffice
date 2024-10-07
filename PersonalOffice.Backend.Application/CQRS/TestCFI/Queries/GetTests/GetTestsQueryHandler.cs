using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTests
{
    internal class GetTestsQueryHandler(
        ILogger<GetTestsQueryHandler> logger,
        ITransportService transportService) : IRequestHandler<GetTestsQuery, IEnumerable<TestVm>>
    {
        private readonly ILogger<GetTestsQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<IEnumerable<TestVm>> Handle(GetTestsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Отправка запроса в микросервис: {mName} метод: {mMethod}",
               MicroserviceNames.DbConnector, "OR_GetTestMenuItems");

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "OR_GetTestMenuItems",
                Data = request.UserID
            }, cancellationToken);

            _logger.LogTrace("Результат получен микросервис: {mName} метод: {mMethod}",
               MicroserviceNames.DbConnector, "OR_GetTestMenuItems");

            var SQLresult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<TestVm>>>(msg.Data);

            if (!SQLresult.Success)
                throw new InvalidOperationException(SQLresult.Message);

            return SQLresult.ReturnValue ?? [];
        }
    }
}
