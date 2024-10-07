using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Data;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetDocFile
{
    internal class GetDocFileQueryHandler(
        ILogger<GetDocFileQueryHandler> logger,
        ITransportService transportService,
        IFileService fileService) : IRequestHandler<GetDocFileQuery, FileVm>
    {
        private readonly ILogger<GetDocFileQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IFileService _fileService = fileService;

        public async Task<FileVm> Handle(GetDocFileQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения пути файла по id: {inf}", request.FileId);

            var fileParam = await GetFilePath(new IdRequest { UniqueId = request.FileId, UserId = request.UserId }, cancellationToken);
            _logger.LogTrace("Путь к файлу получен id: {inf}, путь: {path}. Начало получения файла", request.FileId, fileParam.FilePath);

            var file = await _fileService.GetFileAsync(fileParam.FilePath, cancellationToken);
            _logger.LogTrace("Файл получен id: {inf}, путь: {path}", request.FileId, fileParam.FilePath);

            return new FileVm { Content = file.Content, FileName = request.FileName ?? Path.GetFileName(fileParam.FilePath), ContentType = "multipart/form-data" };
        }

        private async Task<FileDataDto> GetFilePath(IdRequest idrequest, CancellationToken cancellationToken = default)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_GetDocFilePath",
                Data = idrequest
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<DataTable>>(msg.Data);

            if (!sqlResult.Success || sqlResult.ReturnValue?.Rows.Count == 0)
                throw new NotFoundException("Файл не найден");

            return new FileDataDto { FilePath = sqlResult.ReturnValue?.Rows[0]["FilePath"].ToString() ?? "" };
        }
    }
}
