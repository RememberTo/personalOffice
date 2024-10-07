using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Report.General;
using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.File;
using PersonalOffice.Backend.Domain.Entities.Report;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using Convert = PersonalOffice.Backend.Application.Common.Global.Convert;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetCustomReport
{
    internal class GetCustomReportQueryHandler(
        ILogger<GetCustomReportQueryHandler> logger,
        ITransportService transportService) : IRequestHandler<GetCustomReportQuery, CustomReportVm?>
    {
        private readonly ILogger<GetCustomReportQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<CustomReportVm?> Handle(GetCustomReportQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения информации о файле {id} из хранилища MinIO", request.ReportId);

            var fileResult = Convert.DataTo<FileOperationResult>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.FileManager,
                Method = "FileExistsInMinIO",
                Data = new FileReadMinIORequest
                {
                    Bucket = "custom-report",
                    UserId = request.UserId,
                    FileName = request.ReportId
                }
            }, TimeSpan.FromMinutes(1))).Data);

            _logger.LogTrace("Данные по файлу {id} получены, результат: {frs}", request.ReportId, fileResult.Success);

            if (fileResult == null || !fileResult.Success) return null;
            
            var metadata = Convert.DataTo<Dictionary<string, string>>(fileResult.Data ?? throw new InvalidOperationException("Информация о файле отсутсвует"));
            var customReportInfo = JsonConvert.DeserializeObject<CustomReportInfo>(metadata["customreport-info"])
                ?? throw new NotFoundException($"Не удалось получить информацию о файле. ReportId: {request.ReportId}");

            return new CustomReportVm
            {
                ReportId = customReportInfo.ReportId,
                FileName = customReportInfo.Filename
            };
        }
    }
}
