using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Data;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetReportFile
{
    internal class GetReportFileQueryHandler(
        ILogger<GetNotifyFileByIdQueryHandler> logger,
        ITransportService transportService,
        IFileService fileService
        ) : IRequestHandler<GetReportFileQuery, FileVm>
    {
        private readonly ILogger<GetNotifyFileByIdQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IFileService _fileService = fileService;

        public async Task<FileVm> Handle(GetReportFileQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения пути файла по id: {inf}", request.FileId);

            var fileParam = await GetFilePath(new IdRequest { UniqueId = request.FileId, UserId = request.UserId }, request.IsSignFile);
            _logger.LogTrace("Путь к файлу получен id: {inf}, путь: {path}. Начало получения файла", request.FileId, fileParam.FilePath);

            var file = await _fileService.GetFileAsync(fileParam.FilePath, cancellationToken);
            _logger.LogTrace("Файл получен id: {inf}, путь: {path}", request.FileId, fileParam.FilePath);

            return new FileVm { Content = file.Content, FileName = request.FileName ?? Path.GetFileName(fileParam.FilePath), ContentType ="multipart/form-data" };
        }

        private async Task<FileDataDto> GetFilePath(IdRequest request, bool sig, CancellationToken cancellationToken = default)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = sig ? "RP_GetReportFileSigPath" : "RP_GetReportFilePath",
                Data = request
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<DataTable>>(msg.Data);

            if (!sqlResult.Success || sqlResult.ReturnValue?.Rows.Count == 0)
                throw new NotFoundException("Файл не найден");

            return new FileDataDto { FilePath = sqlResult.ReturnValue?.Rows[0]["FilePath"].ToString() ?? ""};
        }
    }
}
