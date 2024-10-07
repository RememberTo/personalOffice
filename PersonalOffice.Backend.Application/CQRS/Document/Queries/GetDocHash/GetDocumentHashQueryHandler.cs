using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocHash
{
    internal class GetDocumentHashQueryHandler(
        ILogger<GetDocumentHashQueryHandler> logger,
        IDocumentService documentService,
        ITransportService transportService) : IRequestHandler<GetDocumentHashQuery, DocHashVm>
    {
        private readonly ILogger<GetDocumentHashQueryHandler> _logger = logger;
        private readonly IDocumentService _documentService = documentService;
        private readonly ITransportService _transportService = transportService;
        public async Task<DocHashVm> Handle(GetDocumentHashQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Получение информации о документе с id: {did}", request.DocumentId);
            var doc = await _documentService.GetDocumentInfoAsync(new DocumentInfoRequest { DocId = request.DocumentId, UserId = request.UserId }, cancellationToken);

            _logger.LogTrace("Получение хэша документа с id: {did}", request.DocumentId);
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.ClientOrder,
                Method = "GetDocHash_POCO",
                Data = request.DocumentId
            }, cancellationToken);

            _logger.LogTrace("Результат получен");
            var result = Common.Global.Convert.DataTo<HashResult>(msg.Data) ?? 
                throw new NotFoundException("Документ не найден");

            if(string.IsNullOrEmpty(result.Hash)) 
                throw new BadRequestException(result.ErrorMessage ?? "Документ не найден");

            return new DocHashVm { Hash = Crypto.GetStringSHA256Hash(result.Hash.Select(x => (byte)x).ToArray()) };
        }
    }
}
