using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.SetStatusDocument
{
    internal class SetStatusDocumentCommandHandler(
        ILogger<SetStatusDocumentCommandHandler> logger,
        IDocumentService documentService,
        ITransportService transportService) : IRequestHandler<SetStatusDocumentCommand, IResult>
    {
        private readonly ILogger<SetStatusDocumentCommandHandler> _logger = logger;
        private readonly IDocumentService _documentService = documentService;
        private readonly ITransportService _transportService = transportService;

        public async Task<IResult> Handle(SetStatusDocumentCommand request, CancellationToken cancellationToken)
        {
            var doc = await _documentService.GetDocumentInfoAsync(new DocumentInfoRequest { DocId = request.DocumentId, UserId = request.UserId }, cancellationToken);

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_SetStatus",
                Data = request,
            }, cancellationToken);

            if (request.Status == 5) // Если отказались
            {
                await _documentService.SetStatusFormAsync(request.DocumentId, 5, cancellationToken);
            }

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<string>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка при установке статуса документа: {msg}", sqlResult?.Message);
                throw new InvalidOperationException($"Ошибка при установке статуса документа");
            }

            return new Result(InternalStatus.Success, "Статус изменен", request.DocumentId);
        }
    }
}
