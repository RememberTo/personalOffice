using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocs
{
    internal class GetDocsQueryHandler(
        ILogger<GetDocsQueryHandler> logger,
        ITransportService transportService) : IRequestHandler<GetDocsQuery, DocumentsInfoVm>
    {
        private readonly ILogger<GetDocsQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<DocumentsInfoVm> Handle(GetDocsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало отправки сообщения в микросервис: {mName} метод: {mMethod} параметры запроса: номер страницы: {numer}, количество элементов: {cnt}",
                MicroserviceNames.DbConnector, "DOC_ClientDocList", request.Page, request.PageSize);

            var msg = await _transportService.RPCServiceAsync(new Message
               {
                   Source = MicroserviceNames.Backend,
                   Destination = MicroserviceNames.DbConnector,
                   Method = "DOC_ClientDocList",
                   Data = request
               }, cancellationToken);

            _logger.LogTrace("Сообщение получено из микросервиса: {mName} метод: {mMethod} параметры запроса: номер страницы: {numer}, количество элементов: {cnt}",
                MicroserviceNames.DbConnector, "DOC_ClientDocList", request.Page, request.PageSize);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<DocumentsInfoVm>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success)
                throw new InvalidOperationException("Не удалось получить данные о документах");


            _logger.LogTrace("Документы получены");

            return sqlResult.ReturnValue!;
        }
    }
}
