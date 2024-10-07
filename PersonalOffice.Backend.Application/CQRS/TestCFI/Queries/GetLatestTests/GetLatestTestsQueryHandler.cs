using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.TestCFI;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetLatestTests
{
    internal class GetLatestTestsQueryHandler(
        ILogger<GetLatestTestsQueryHandler> logger,
        ITransportService transportService) 
        : IRequestHandler<GetLatestTestsQuery, LatestTestsVm>
    {
        private readonly ILogger<GetLatestTestsQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<LatestTestsVm> Handle(GetLatestTestsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Отправка запроса в микросервис: {mName} метод: {mMethod}",
               MicroserviceNames.DbConnector, "OR_TestList4User");

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "OR_TestList4User",
                Data = request
            }, cancellationToken);


            _logger.LogTrace("Результат получен микросервис: {mName} метод: {mMethod}",
               MicroserviceNames.DbConnector, "OR_TestList4User");

            var SQLresult = Common.Global.Convert.DataTo<SQLOperationResult<DbTestsCFI>>(msg.Data);

            if (!SQLresult.Success || SQLresult.ReturnValue is null)
                throw new InvalidOperationException(SQLresult.Message);

            var tests = Common.Global.Convert.DataTableToEnumerable<TestInfoVm>(SQLresult.ReturnValue.Tests);

            return new LatestTestsVm { PageCount = SQLresult.ReturnValue.PageCount, Tests = tests };
        }
    }
}
