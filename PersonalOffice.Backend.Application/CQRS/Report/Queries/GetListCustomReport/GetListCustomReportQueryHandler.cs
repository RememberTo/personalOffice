using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Report.General;
using PersonalOffice.Backend.Application.CQRS.Report.Queries.GetCustomReport;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.File;
using PersonalOffice.Backend.Domain.Entities.Report;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetListCustomReport
{
    internal class GetListCustomReportQueryHandler(
        ILogger<GetListCustomReportQueryHandler> logger,
        ITransportService transportService,
        IUserService userService) : IRequestHandler<GetListCustomReportQuery, IEnumerable<CustomReportVm>>
    {
        private readonly ILogger<GetListCustomReportQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<CustomReportVm>> Handle(GetListCustomReportQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения списка доступных отчетов из микросервис {mcr} GetFileListInMinIO", MicroserviceNames.FileManager);

            var files = Common.Global.Convert.DataTo<IList<FileInfoMinIOResult>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.FileManager,
                Method = "GetFileListInMinIO",
                Data = new GetFileListQuery
                {
                    Bucket = "custom-report",
                    UserId = request.UserId,
                }
            }, TimeSpan.FromMinutes(10))).Data);

            _logger.LogTrace("Список доступных отчетов получен");
            var info = new Dictionary<FileInfoMinIOResult, CustomReportInfo>();
            foreach (var file in files)
            {
                try
                {
                    var customReportInfo = JsonConvert.DeserializeObject<CustomReportInfo>(file.MetaData["customreport-info"])
                       ?? throw new Exception($"Нет необходимых метаданных файла: customreport-info для filename: {file.FileName}");
                    info.Add(file, customReportInfo);
                }
                catch (Exception ex)
                {
                    _logger.LogTrace("Ошибка: {msg}", ex.Message);
                } 
            }

            _logger.LogTrace("Информация об отчетах сформирована");
            var uniqContractIds = info.Select(file => file.Value.ContractId).Distinct();
            var docNums = new Dictionary<int, string>();

            foreach (var contractId in uniqContractIds)
            {
                docNums[contractId] = await _userService.GetDocNumAsync(contractId, request.UserId, cancellationToken);
            }

            _logger.LogTrace("Номера документов получены");

            var result = info.Select(obj =>
            {
                try
                {
                    var date = obj.Value.BeginDate == obj.Value.EndDate ? obj.Value.BeginDate.ToString("dd.MM.yyyy")
                        : obj.Value.BeginDate.ToString("dd.MM.yyyy") + "-" + obj.Value.EndDate.ToString("dd.MM.yyyy");

                    return new CustomReportVm
                    {
                        ReportId = obj.Value.ReportId,
                        Name = docNums[obj.Value.ContractId] + " " + ReportType.Names[obj.Value.ReportType] + " за " + date,
                        FileName = obj.Value.Filename
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogTrace("Ошибка: {msg}", ex.Message);
                    return null;
                }
            }).Where(x => x != null);

            return result!;
        }
    }
}
