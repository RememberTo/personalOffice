using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Extesions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById.Vm;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using PersonalOffice.Backend.Domain.Entities.Document.Info;
using PersonalOffice.Backend.Domain.Entities.Document.Serialization;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById
{
    internal class GetDocByIdQueryHandler(
        ILogger<GetDocByIdQueryHandler> logger,
        IMapper mapper,
        IFileService fileService,
        IUserService userService,
        IDocumentService documentService,
        ITransportService transportService) : IRequestHandler<GetDocByIdQuery, DocumentBaseVm>
    {
        private readonly ILogger<GetDocByIdQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IFileService _fileService = fileService;
        private readonly IUserService _userService = userService;
        private readonly IDocumentService _documentService = documentService;
        private readonly ITransportService _transportService = transportService;

        public async Task<DocumentBaseVm> Handle(GetDocByIdQuery request, CancellationToken cancellationToken)
        {
            var docRequest = new DocumentInfoRequest { DocId = request.DocId, UserId = request.UserId };

            var user = await _userService.GetGeneralUserInfoAsync(request.UserId, cancellationToken);
            _logger.LogTrace("Получение общей информации о документе");

            var doc = await _documentService.GetDocumentInfoAsync(docRequest, cancellationToken, false);

            var documentVm = await GetDocByIdAsync(doc, docRequest, cancellationToken);

            documentVm.Contract = doc.Contract!;
            documentVm.TypeId = doc.TypeID;
            documentVm.TypeName = doc.TypeName!;
            documentVm.Date = doc.Date!;
            documentVm.Status = doc.Status;
            documentVm.StatusDate = doc.StatusDate!;
            documentVm.DocStatusComment = doc.DocStatusComment;
            documentVm.NeedClientSign = doc.NeedClientSign;
            documentVm.IsPhysic = user.IsPhysic;

            return documentVm;
        }

        private async Task<DocumentBaseVm> GetDocByIdAsync(DocumentInfoDataTable doc, DocumentInfoRequest query, CancellationToken cancellationToken = default)
        {
            switch (doc.TypeID)
            {
                case 1:
                case 3:
                    var fileResult = await _fileService.GetFileAsync(doc.FilePath, cancellationToken);
                    if (fileResult is null || !fileResult.Success)
                        throw new NotFoundException($"Файл {doc.FilePath} не найден");

                    if (doc.TypeID == 1)
                        return _mapper.Map<InvestProfileAnketaDocumentVm>(GetInvestProfileAnketaInfo(fileResult.Content));
                    else if (doc.TypeID == 3)
                        return _mapper.Map<ArbitraryDocumentVm>(GetDocumentArbitaryInfo(fileResult.Content));
                    break;

                case 2:
                    return _mapper.Map<InvestProfileSpravkaDocumentVm>(await GetInvestProfileSpravkaInfo(query, cancellationToken));

                default:
                    throw new InvalidOperationException($"Неподдерживаемый тип документа: {doc.TypeID}");
            }
            throw new InvalidOperationException();
        }

        private async Task<InvestProfileSparvkaInfo> GetInvestProfileSpravkaInfo(DocumentInfoRequest query, CancellationToken cancellationToken = default)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_GetFileList",
                Data = query
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<DocumentFileInfo>>>(msg.Data);

            if (sqlResult is null || !sqlResult.Success) throw new NotFoundException("Документ не найден");

            return new InvestProfileSparvkaInfo { Files = sqlResult.ReturnValue!.ToList() };
        }

        private DocumentArbitraryInfo GetDocumentArbitaryInfo(byte[] content)
        {
            var DocArbitrary = XmlHelper.DeserializeObject<DocumentArbitrary>(content);
            return new DocumentArbitraryInfo()
            {
                DocName = DocArbitrary.DocName,
                DocComment = DocArbitrary.DocComment ?? string.Empty,
                FilesHash = DocArbitrary.ListFileHash
                    .Select(el => new DocElement<string, string>
                    {
                        Key = el[..el.IndexOf('{')],
                        Value = el[el.IndexOf('{')..],
                    })
                    .ToList()
            };
        }

        private InvestProfileAnketaInfo GetInvestProfileAnketaInfo(byte[] content)
        {
            var docIP = XmlHelper.DeserializeObject<DocumentInvestProfile>(content);
            var test = docIP.Content.RemoveAll(x => x.Key.In(2787, 2788));
            return new InvestProfileAnketaInfo
            {
                Elements = docIP.Content
                .Select(x => new DocElement<string, string>
                {
                    Key = Data.InvestProfileFields.FirstOrDefault(y => y.Value == x.Key).Key,
                    Value = x.Value
                })
                .Where(element => element.Key != null)
                .ToList()
            };
        }
    }

}
