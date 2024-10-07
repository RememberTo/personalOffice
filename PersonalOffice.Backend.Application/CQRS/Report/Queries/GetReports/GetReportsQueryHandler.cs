using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Report.General;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Data;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetReports
{
    internal class GetReportsQueryHandler(
        ILogger<GetReportsQueryHandler> logger,
        ITransportService transportService) : IRequestHandler<GetReportsQuery, ReportsVm>
    {
        private readonly ILogger<GetReportsQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<ReportsVm> Handle(GetReportsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало отправки сообщения в микросервис: {mName} метод: {mMethod} параметры запроса: номер страницы: {numer}, количество элементов: {cnt}",
                MicroserviceNames.DbConnector, "RP_GetReportList4User", request.PageNum, request.Count);

            var msg = await _transportService.RPCServiceAsync(
               new Message
               {
                   Source = MicroserviceNames.Backend,
                   Destination = MicroserviceNames.DbConnector,
                   Method = "RP_GetReportList4User",
                   Data = request
               }, cancellationToken);

            _logger.LogTrace("Сообщение получено из микросервиса: {mName} метод: {mMethod} параметры запроса: номер страницы: {numer}, количество элементов: {cnt}",
                MicroserviceNames.DbConnector, "RP_GetReportList4User", request.PageNum, request.Count);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<DataTable>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success)
                throw new InvalidOperationException("Не удалось получить данные");

            var reports = Common.Global.Convert.DataTableToEnumerable<ReportVm>(sqlResult.ReturnValue ?? new DataTable());
            reports
                .ToList()
                .ForEach(x => x.FileExt = Path.GetExtension(x.File)?.Replace(".", ""));


            _logger.LogTrace("Отчеты получены");

            return new ReportsVm { TotalCount = await GetCountReports(request.UserId), Reports = reports };
        }

        private async Task<long> GetCountReports(int userId)
        {
            var msg = await _transportService.RPCServiceAsync(
               new Message
               {
                   Source = MicroserviceNames.Backend,
                   Destination = MicroserviceNames.DbConnector,
                   Method = "RP_GetReportsCount4User",
                   Data = new GetCountReportsQuery { PeriodID = 0, UserId = userId }
               }, TimeSpan.FromSeconds(5));

            return Common.Global.Convert.DataTo<SQLOperationResult<long>>(msg.Data).ReturnValue;
        }
    }
}
