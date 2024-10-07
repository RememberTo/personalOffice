using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Extesions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.File.Commands;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.Document;
using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Entities.Document.Serialization;
using PersonalOffice.Backend.Domain.Entities.File;
using PersonalOffice.Backend.Domain.Entities.POClientOrder;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateArbitraryDocument
{
    internal class CreateArbitraryDocumentCommandHandler(
        ILogger<CreateArbitraryDocumentCommandHandler> logger,
        IUserService userService,
        IDocumentService documentService,
        IFileService fileService,
        ITransportService transportService) : IRequestHandler<CreateArbitraryDocumentCommand, IResult>
    {
        private readonly ILogger<CreateArbitraryDocumentCommandHandler> _logger = logger;
        private readonly IUserService _userService = userService;
        private readonly IDocumentService _documentService = documentService;
        private readonly IFileService _fileService = fileService;
        private readonly ITransportService _transportService = transportService;

        public async Task<IResult> Handle(CreateArbitraryDocumentCommand request, CancellationToken cancellationToken)
        {
            var docNum = await _userService.GetDocNumAsync(request.ContractId, request.UserId, cancellationToken); //Проверяется валидность указанного contract id
            var user = await _userService.GetGeneralUserInfoAsync(request.UserId, cancellationToken);
            var personCode = await _userService.GetPersonCodeAsync(request.ContractId, cancellationToken);
            var files = GetDataFiles(request.UploadFiles).ToList();

            var docArbitrary = new DocumentArbitrary()
            {
                ContractID = request.ContractId,
                DocNum = docNum,
                PersonName = user.FullName,
                Date = DateTime.Now,
                TypeID = 3,
                Code = personCode,
                DocName = request.Name,
                DocComment = request.Comment,
                ListFileHash = GetHashFiles(files).ToList(),
            };

            var docId = await SaveArbitraryDocumentPO(docArbitrary, cancellationToken);

            var randomFolder = Crypto.GetHexStringFromByte(Guid.NewGuid().ToByteArray());

            foreach (var file in files)
            {
                var path = $@"\i\docs\{docArbitrary.Code}_{docId}\{randomFolder}\{file.FileName}".Replace(['/', ':', '*', '?', '"', '<', '>', '|', '+'], '_');

                await AddFile2Doc(new AddFileRequest
                {
                    EntityID = docId,
                    FileContent = file.Data,
                    FileName = path,
                }, cancellationToken);
            }

            return new Result(InternalStatus.Created, "Документ создан", docId);
        }

        private IEnumerable<FileData> GetDataFiles(ICollection<UploadFile> uploadFiles)
        {
            foreach (var file in uploadFiles)
            {
                using var bReader = new BinaryReader(file.Stream);

                yield return new FileData
                {
                    FileName = file.FileName,
                    Data = bReader.ReadBytes((int)file.Length),
                };
            }
        }

        private IEnumerable<string> GetHashFiles(IEnumerable<FileData> fileDatas)
        {
            foreach (var file in fileDatas)
            {
                var checkSum = MD5.HashData(file.Data);
                var hash = BitConverter.ToString(checkSum).Replace("-", string.Empty);
                var item = $"{file.FileName} {{{hash}}}";

                yield return item;
            }
        }

        public async Task AddFile2Doc(AddFileRequest addFileRequest, CancellationToken cancellationToken = default)
        {
            var entityResult = Common.Global.Convert.DataTo<EntityOperationResult<string?>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.ClientOrder,
                Method = "AddFile2Doc_POCO",
                Data = addFileRequest,
            }, cancellationToken)).Data);

            if (entityResult == null || !entityResult.IsSuccess) throw new InvalidOperationException($"Не удалось добавить файл к документу");
        }

        public async Task<int> SaveArbitraryDocumentPO(DocumentArbitrary Doc, CancellationToken cancellationToken = default)
        {
            byte[] xmlByte = XmlHelper.SerializeObject(Doc);
            string xmlString = Encoding.UTF8.GetString(xmlByte);

            var docId = await _documentService.AddDocumentAsync(new AddDocumentRequest
            {
                Content = xmlString,
                ContractID = Doc.ContractID,
                FilePath = "",
                Hash = Crypto.GetStringSHA256Hash(xmlByte),
                Name = Doc.DocName,
                ParentID = null,
                TypeID = 3
            }, cancellationToken);

            string FilePath = @"\i\docs\" + DateTime.Today.Year.ToString("0000") + "_" + DateTime.Today.Month.ToString("00") + @"\" + Doc.Code + "_" + docId.ToString() + ".xml";

            _logger.LogTrace("SaveArbitraryDocument 3 вызов WriteFile");

            await _fileService.WriteFileAsync(new FileOperationParameters()
            {
                Name = FilePath,
                Content = xmlByte,
                Overwrite = false
            });

            _logger.LogTrace("SaveArbitraryDocument 4 вызов DOC_SetFilePath");
            await _documentService.SetFilePathDocumentAsync(new DocumentFilePathRequest()
            {
                DocumentId = docId,
                FilePath = FilePath
            }, cancellationToken);

            return docId;
        }
    }
}
