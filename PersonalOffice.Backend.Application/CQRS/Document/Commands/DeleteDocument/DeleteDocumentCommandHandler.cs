using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Entities.SQL;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.DeleteDocument
{
    internal class DeleteDocumentCommandHandler(
        ILogger<DeleteDocumentCommandHandler> logger,
        IDocumentService documentService,
        IFileService fileService,
        ITransportService transportService) : IRequestHandler<DeleteDocumentCommand, IResult>
    {
        private readonly ILogger<DeleteDocumentCommandHandler> _logger = logger;
        private readonly IDocumentService _documentService = documentService;
        private readonly IFileService _fileService = fileService;
        private readonly ITransportService _transportService = transportService;

        public async Task<IResult> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = await _documentService.GetDocumentInfoAsync(new DocumentInfoRequest { DocId = request.DocumentId, UserId = request.UserId}, cancellationToken); 

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_DeleteDoc",
                Data = new IntRequest { Value = request.DocumentId },
            }, cancellationToken)).Data);

            if (sqlResult == null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка при удалении документа: {msg}", sqlResult?.Message);
                throw new InvalidOperationException($"Ошибка при удалении документа");
            }
            await _fileService.DeleteAsync(doc.FilePath);

            return new Result(InternalStatus.Success, data: doc);
        }
    }
}
