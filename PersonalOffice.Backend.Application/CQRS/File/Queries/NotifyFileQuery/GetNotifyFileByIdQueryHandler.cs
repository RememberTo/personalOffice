using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries
{
    internal class GetNotifyFileByIdQueryHandler(
        ILogger<GetNotifyFileByIdQueryHandler> logger,
        ITransportService transportService,
        IFileService fileService
        ) : IRequestHandler<GetNotifyFileByIdQuery, FileVm>
    {
        private readonly ILogger<GetNotifyFileByIdQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IFileService _fileService = fileService;

        public async Task<FileVm> Handle(GetNotifyFileByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения пути файла по id: {inf}", request.FileId);
            
            var fileData = await GetFilePath(request, cancellationToken);
            _logger.LogTrace("Путь к файлу получен id: {inf}, путь: {path}. Начало получения файла", request.FileId, fileData.FilePath);
            
            var file = await _fileService.GetFileAsync(fileData.FilePath, cancellationToken);
            _logger.LogTrace("Файл получен id: {inf}, путь: {path}", request.FileId, fileData.FilePath);

            return new FileVm 
            { 
                Content = file.Content, 
                ContentType = "multipart/form-data", 
                FileName = fileData.Name ?? "file"
            };
        }

        private async Task<FileDataDto> GetFilePath(GetNotifyFileByIdQuery request, CancellationToken cancellationToken)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_NotifyFilePathForUserID",
                Data = request
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<FileDataDto>>(msg.Data);

            if (!sqlResult.Success)
                throw new NotFoundException("Файл не найден");

            return sqlResult.ReturnValue ?? throw new NotFoundException("Файл не найден"); ;
        }
    }
}
