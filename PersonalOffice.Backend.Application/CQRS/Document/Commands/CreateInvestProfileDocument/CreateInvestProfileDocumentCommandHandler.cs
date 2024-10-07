using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.Document;
using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using PersonalOffice.Backend.Domain.Entities.Document.Serialization;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Text;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateInvestProfileDocument
{
    internal class CreateInvestProfileDocumentCommandHandler(
        ILogger<CreateInvestProfileDocumentCommandHandler> logger,
        IUserService userService,
        IDocumentService documentService,
        IFileService fileService) : IRequestHandler<CreateInvestProfileDocumentCommand, IResult>
    {
        private readonly ILogger<CreateInvestProfileDocumentCommandHandler> _logger = logger;
        private readonly IUserService _userService = userService;
        private readonly IDocumentService _documentService = documentService;
        private readonly IFileService _fileService = fileService;

        public async Task<IResult> Handle(CreateInvestProfileDocumentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Получение информации о пользователе");
            var user = await _userService.GetGeneralUserInfoAsync(request.UserId, cancellationToken);

            var investProfileDoc = new DocumentInvestProfile()
            {
                ContractID = request.ContractId,
                Date = DateTime.Now,
                TypeID = 2,
                Content = request.Params
                    .Where(paramName => Data.InvestProfileFields.ContainsKey(paramName.Key) && !string.IsNullOrEmpty(paramName.Key))
                    .Select(paramName => new DocElement<int, string> { Key = Data.InvestProfileFields[paramName.Key], Value = paramName.Value })
                    .ToList(),
                Code = await _userService.GetPersonCodeAsync(request.ContractId, cancellationToken)
            };
            investProfileDoc.Content.Add(new DocElement<int, string> { Key = user.IsPhysic ? 2787 : 2788, Value = investProfileDoc.Content.Single(x => x.Key == 2789).Value });
            investProfileDoc.Content.Add(new DocElement<int, string> { Key = 2773, Value = user.IsPhysic ? "1" : "2" });
            _logger.LogTrace("Документ сформирован");

            var docId = await SaveInvestProfileDocument(investProfileDoc, cancellationToken);
            _logger.LogTrace("Документ создан id: {did}", docId);

            return new Result(InternalStatus.Created, "Документ создан" , docId);
        }

        private async Task<int> SaveInvestProfileDocument(DocumentInvestProfile document, CancellationToken cancellationToken = default)
        {
            var xmlByte = XmlHelper.SerializeObject(document);
            var xmlString = Encoding.UTF8.GetString(xmlByte);

            var docId = await _documentService.AddDocumentAsync(new AddDocumentRequest
            {
                Content = xmlString,
                ContractID = document.ContractID,
                FilePath = "",
                Hash = Crypto.GetStringSHA256Hash(xmlByte),
                Name = "Анкета для определения инвестиционного профиля Клиента",
                ParentID = null,
                TypeID = 1
            }, cancellationToken);

            _logger.LogTrace("Документ добавлен id: {did}", docId);

            var filePath = @"\i\docs\" + DateTime.Today.Year.ToString("0000") +
                "_" + DateTime.Today.Month.ToString("00") + @"\" + document.Code + "_" + docId + ".xml";

            await _fileService.WriteFileAsync(new FileOperationParameters
            {
                Name = filePath,
                Content = xmlByte,
                Overwrite = false
            });

            _logger.LogTrace("Документ записан id: {did}", docId);

            await _documentService.SetFilePathDocumentAsync(new DocumentFilePathRequest()
            {
                DocumentId = docId,
                FilePath = filePath
            }, cancellationToken);

            _logger.LogTrace("Добавлен путь к файлу документа записан id: {did}", docId);

            return docId;
        }
    }
}
