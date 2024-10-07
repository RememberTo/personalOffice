using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateArbitraryDocument;
using PersonalOffice.Backend.Application.Services.Base;
using PersonalOffice.Backend.Domain.Entites.Document;
using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entites.User;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;
using PersonalOffice.Backend.Domain.Entities.Document.Serialization;
using PersonalOffice.Backend.Domain.Entities.File;
using PersonalOffice.Backend.Domain.Entities.Mail;
using PersonalOffice.Backend.Domain.Entities.Notify;
using PersonalOffice.Backend.Domain.Entities.Report;
using PersonalOffice.Backend.Domain.Entities.SQL;
using PersonalOffice.Backend.Domain.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Collections;
using System.Data;
using System.Text;
using System.Threading;
using Convert = PersonalOffice.Backend.Application.Common.Global.Convert;

namespace PersonalOffice.Backend.Application.Services
{
    internal class DocumentService(
        IService<DocumentService> service,
        ICacheManager cache,
        IConfiguration configuration,
        IMailService mailService,
        IFileService fileService) : IDocumentService
    {
        private readonly ICacheManager _cache = cache;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMailService _mailService = mailService;
        private readonly IFileService _fileService = fileService;
        private readonly ILogger<DocumentService> _logger = service.Logger;
        private readonly ITransportService _transportService = service.TransportService;

        #region Private Methods

        /// <summary>
        /// Устанавливает статус, для анкеты и справки, а также отправляет уведомление
        /// </summary>
        /// <param name="signInfo"></param>
        /// <param name="pathAnketa"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task SetAllStatusSignAsync(DocumentSignInfo signInfo, string pathAnketa, CancellationToken cancellationToken)
        {
            var sqlResultNotify = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "CreateNotificationInvestAnketa",
                Data = new NotifyInvestmentAnketaRequest
                {
                    UserId = signInfo.UserId,
                    FilePath = pathAnketa
                }
            }, cancellationToken)).Data);

            if (sqlResultNotify is null || !sqlResultNotify.Success)
            {
                _logger.LogError("Ошибка при создании уведомления в БД:  {msg}", sqlResultNotify?.Message);
            }

            await SetStatusFormAsync(signInfo.DocId, 1, cancellationToken);

            var sqlResultSetStatus = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_SetStatus",
                Data = new DocumentStatusRequest() { DocId = signInfo.DocId, UserId = signInfo.UserId, Status = 6 }
            }, cancellationToken)).Data);

            if (sqlResultSetStatus is null || !sqlResultSetStatus.Success)
            {
                _logger.LogError("Ошибка при изменеии статуса справки в БД: {msg}", sqlResultSetStatus?.Message);
                throw new InvalidOperationException("Ошибка при изменеии статуса справки");
            }
        }

        /// <summary>
        /// Отправляет документ по почте
        /// </summary>
        /// <param name="documentMailInfo"></param>
        /// <returns></returns>
        private async Task SendDocumentMailAsync(DocumentMailInfo documentMailInfo)
        {
            var doc = await GetDocumentInfoAsync(new DocumentInfoRequest { DocId = documentMailInfo.DocumentId, UserId = documentMailInfo.UserId });

            var message = new MailMessage()
            {
                Application = MicroserviceNames.Backend,
                Subject = "Загружен новый документ"
            };

            var sb = new StringBuilder();
            sb.AppendLine("<html><body>");
            sb.AppendLine("<h3>Загружен документ из ЛК</h3>");
            sb.AppendLine("<table cellpadding=3 cellspacing=1 border=1>");
            sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", "Договор", doc.Contract);
            sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", "Тип", doc.TypeName);
            sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", "Дата подачи", doc.Date);
            sb.AppendFormat("<tr><td>{0}</td><td><a href='http://sf.solid.com.ru/docs/clients/form32.aspx?FormID={1}&FormTypeId=32&ContractID={2}'>" +
                "{1}</a></td></tr>", "Открыть в СФ", documentMailInfo.FormId.ToString(), documentMailInfo.ContractId.ToString());
            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            message.HtmlBody = sb.ToString();
            var branchId = doc.BranchId ?? "17";
            message.To.Add(_configuration["Application:Mail:Doc." + branchId] ?? _configuration["Application:Mail:Default"]!);

            _mailService.TrySend(message, out _);
        }

        /// <summary>
        /// Подписывает документ
        /// </summary>
        /// <param name="signInfo"></param>
        /// <param name="file"></param>
        /// <param name="filePathSign"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task AddSignatureAsync(DocumentSignInfo signInfo, byte[] file, string filePathSign, CancellationToken cancellationToken = default)
        {
            var signData = signInfo.SignType == SignType.SmsCode
                 ? Crypto.HmacSignature(signInfo.Signature, file)
                 : Encoding.UTF8.GetBytes(signInfo.Signature);

            await _fileService.WriteFileAsync(new FileOperationParameters { Content = signData, Name = filePathSign, Overwrite = false });

            var sqlResult = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = signInfo.SignType == SignType.SmsCode ? "DOC_SignCode" : "DOC_SignCert",
                Data = new DocumentSignRequest
                {
                    DocId = signInfo.DocId,
                    UserId = signInfo.UserId,
                    Code = signInfo.Signature,
                    FilePath = filePathSign
                }
            }, cancellationToken)).Data);

            if (sqlResult is null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка при сохранении подписи произвольного документа в БД: {msg}", sqlResult?.Message);
                throw new InvalidOperationException("Ошибка, не удалось сохранить подпись");
            }
        }

        #endregion

        public async Task SetStatusFormAsync(int docId, int statusId, CancellationToken cancellationToken = default)
        {
            var sqlResultSetStatusForm = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_FormSetStatus",
                Data = new SetStatusFormRequest() { DocId = docId, StatusId = statusId }
            }, cancellationToken)).Data);

            if (sqlResultSetStatusForm is null || !sqlResultSetStatusForm.Success)
            {
                _logger.LogError("Ошибка при изменеии статуса анкеты в БД:  {msg}", sqlResultSetStatusForm?.Message);
                throw new InvalidOperationException("Ошибка при изменеии статуса анкеты");
            }
        }

        public async Task<int> AddDocumentAsync(AddDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<int>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_AddDoc",
                Data = request,
            }, cancellationToken)).Data);

            if (sqlResult == null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка при записи документа в БД: {err}", sqlResult?.Message);
                throw new InvalidOperationException($"Ошибка при записи документа");
            }

            return sqlResult.ReturnValue;
        }

        public async Task<DocumentInfoDataTable> GetDocumentInfoAsync(DocumentInfoRequest documentInfo, CancellationToken cancellationToken = default, bool isCached = true)
        {
            var cacheKey = Convert.CachedKey("docInfo", documentInfo.UserId, documentInfo.DocId);
            if (isCached)
            {
                var cachedDocInfo = await _cache.Get<DocumentInfoDataTable>(cacheKey);

                if (cachedDocInfo != null)
                    return cachedDocInfo;
            }

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_GetInfo",
                Data = documentInfo
            }, cancellationToken);

            var sqlResult = Convert.DataTo<SQLOperationResult<IEnumerable<DocumentInfoDataTable>>>(msg.Data);

            if (sqlResult == null || !sqlResult.Success)
                throw new InvalidOperationException("Не удалось получить данные о документе");

            if (sqlResult.ReturnValue is null || !sqlResult.ReturnValue.Any())
                throw new NotFoundException("Документ не найден");

            _logger.LogTrace("Документ получен");

            var doc = sqlResult.ReturnValue!.First() ?? throw new NotFoundException($"Документ {documentInfo.DocId} не найден");

            await _cache.Set(cacheKey, doc, TimeSpan.FromMinutes(5));

            return doc;
        }

        public async Task SignDocumentAsync(int docType, DocumentSignInfo documentInfo, CancellationToken cancellationToken = default)
        {
            switch (docType)
            {
                case 1:
                    await SignInvestProfileDocumentAsync(documentInfo, cancellationToken);
                    break;
                case 2:
                   await SignInvestSpravkaDocumentAsync(documentInfo, cancellationToken);
                    break;
                case 3:
                    await SignArbitraryDocumentAsync(documentInfo, cancellationToken);
                    break;
                default:
                    throw new BadRequestException("Неподдерживаемый тип документа");
            }
        }

        public async Task SignArbitraryDocumentAsync(DocumentSignInfo signInfo, CancellationToken cancellationToken = default)
        {
            var fileResult = await _fileService.GetFileAsync(signInfo.DocFilePath, cancellationToken);
            var filePathSign = signInfo.DocFilePath.Replace(".xml", ".sig");

            await AddSignatureAsync(signInfo, fileResult.Content, filePathSign, cancellationToken);

            var docArbitrary = XmlHelper.DeserializeObject<DocumentArbitrary>(fileResult.Content);
            docArbitrary.DocId = signInfo.DocId;

            await _transportService.SendMessageAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.ClientOrder,
                Method = "SendArbitraryDocument2Mail_POCO",
                Data = docArbitrary
            });
        }

        public async Task SetFilePathDocumentAsync(DocumentFilePathRequest pathCommand, CancellationToken cancellationToken = default)
        {
            var sqlResultSetPath = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_SetFilePath",
                Data = pathCommand
            }, cancellationToken)).Data);

            if (sqlResultSetPath == null || !sqlResultSetPath.Success)
            {
                _logger.LogError("Ошибка при записи произвольного документа в БД: {err}", sqlResultSetPath?.Message);
                throw new InvalidOperationException($"Ошибка при записи произвольного документа");
            }
        }

        public async Task SignInvestProfileDocumentAsync(DocumentSignInfo signInfo, CancellationToken cancellationToken = default)
        {
            var sqlResultFilePath = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_GetFilePath",
                Data = new IntRequest { Value = signInfo.DocId },
            }, cancellationToken)).Data);

            if (sqlResultFilePath is null || !sqlResultFilePath.Success || sqlResultFilePath.ReturnValue is null)
            {
                _logger.LogError("Ошибка при получении пути к файлу документа: {msg}", sqlResultFilePath?.Message);
                throw new InvalidOperationException("Информация о дкументе отсутствует");
            }

            var fileResult = await _fileService.GetFileAsync(sqlResultFilePath.ReturnValue, cancellationToken);
            var doc = XmlHelper.DeserializeObject<DocumentInvestProfile>(fileResult.Content);
            var filePathSign = @"\i\docs\" + DateTime.Today.Year.ToString("0000") + "_" + DateTime.Today.Month.ToString("00") + @"\" + doc.Code + "_" + signInfo.DocId.ToString() + ".sig";
            
            var sqlResultInvestForm = Convert.DataTo<SQLOperationResult<int>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_AddInvestForm",
                Data = new AddFormRequest { DocId = signInfo.DocId, ContractId = doc.ContractID },
            }, cancellationToken)).Data);

            if (sqlResultInvestForm is null || !sqlResultInvestForm.Success)
            {
                _logger.LogError("Ошибка при создании инвест-формы в СФ: {msg}", sqlResultInvestForm?.Message);
                throw new InvalidOperationException("Ошибка, не удалось создать инвест форму");
            }

            var formId = sqlResultInvestForm.ReturnValue;

            await AddSignatureAsync(signInfo, fileResult.Content, filePathSign, cancellationToken);

            foreach (var detail in doc.Content)
            {
                var sqlResultFormDetail = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
                {
                    Source = MicroserviceNames.Backend,
                    Destination = MicroserviceNames.DbConnector,
                    Method = "DOC_AddInvestFormDetail",
                    Data = new InvestFormDetailRequest
                    {
                        FormId = formId,
                        FieldId = detail.Key,
                        Value = detail.Value,
                        Description = null
                    }
                }, cancellationToken)).Data);

                if (sqlResultFormDetail is null || !sqlResultFormDetail.Success)
                {
                    _logger.LogError("Ошибка при добавлении детализации инвест-формы:  {msg}", sqlResultInvestForm?.Message);
                    throw new InvalidOperationException("Ошибка, при добавлении детализации инвест-формы");
                }
            }

            await SendDocumentMailAsync(new DocumentMailInfo { DocumentId = signInfo.DocId, FormId = formId, ContractId = doc.ContractID, UserId = signInfo.UserId });

            var anketa = await GetContentReportDocumentAsync(formId, "GetInvestProfileAnketa");
            var spravka = await GetContentReportDocumentAsync(formId, "GetInvestProfileSpravka");

            var path = @"\i\docs\" + DateTime.Today.Year.ToString("0000") + "_" + DateTime.Today.Month.ToString("00") + @"\";
            var pathAnketa = path + doc.Code + "_anketa_" + signInfo.DocId.ToString() + ".pdf";
            var pathSpravka = path + doc.Code + "_spravka_" + signInfo.DocId.ToString() + ".pdf";

            await _fileService.WriteFileAsync(new FileOperationParameters
            {
                Name = pathAnketa,
                Content = anketa,
            });
            await _fileService.WriteFileAsync(new FileOperationParameters
            {
                Name = pathSpravka,
                Content = spravka,
            });

            var docId = await AddDocumentAsync(new AddDocumentRequest
            {
                Content = string.Empty,
                ContractID = doc.ContractID,
                FilePath = path + doc.Code + "_spravka_" + signInfo.DocId.ToString() + ".pdf",
                Hash = Crypto.GetStringSHA256Hash(spravka),
                Name = "Справка об инвестиционном профиле",
                ParentID = signInfo.DocId,
                TypeID = 2
            }, cancellationToken);


            await AddFile2DocumentAsync(signInfo.DocId, pathAnketa, cancellationToken);
            await AddFile2DocumentAsync(docId, pathSpravka, cancellationToken);


            await SetAllStatusSignAsync(signInfo, pathAnketa, cancellationToken);
        }

        public async Task SignInvestSpravkaDocumentAsync(DocumentSignInfo signInfo, CancellationToken cancellationToken = default)
        {
            var sqlResult = Convert.DataTo<SQLOperationResult<bool>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_OwnDoc",
                Data = new OwnDocRequest
                {
                    DocId = signInfo.DocId,
                    UserId = signInfo.UserId,
                }
            }, cancellationToken)).Data);

            if (sqlResult is null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка при получении сведений о принадлежности документа: {msg}", sqlResult?.Message);
                throw new InvalidOperationException("Ошибка при получении сведений о принадлежности документа");
            }
            if (!sqlResult.ReturnValue)
            {
                throw new InvalidOperationException("Нет доступа к данному документу!");
            }

            var sqlResultFilePath = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_GetFilePath",
                Data = new IntRequest { Value = signInfo.DocId },
            }, cancellationToken)).Data);

            if (sqlResultFilePath is null || !sqlResultFilePath.Success || sqlResultFilePath.ReturnValue is null)
            {
                _logger.LogError("Ошибка при получении пути к файлу справки: {msg}", sqlResultFilePath?.Message);
                throw new InvalidOperationException("Информация о документе отсутствует");
            }

            var fileResult = await _fileService.GetFileAsync(signInfo.DocFilePath, cancellationToken);
            string filePathSign = @"\i\docs\" + DateTime.Today.Year.ToString("0000") + "_" + DateTime.Today.Month.ToString("00") + @"\" + "_" + signInfo.DocId.ToString() + ".sig";

            await AddSignatureAsync(signInfo, fileResult.Content, filePathSign, cancellationToken);

            await SetStatusFormAsync(signInfo.DocId, 2, cancellationToken);
        }

        public async Task<byte[]> GetContentReportDocumentAsync(int formId, string Method)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.ReportMaster,
                Method = Method,
                Data = new IntRequest { Value = formId }
            }, TimeSpan.FromMinutes(2));

            var result = Convert.DataTo<ObjectBytes>(msg.Data);
            var decodedData = System.Convert.FromBase64String(result.Value);

            return decodedData;
        }

        public async Task AddFile2DocumentAsync(int docId, string path, CancellationToken cancellationToken = default)
        {
            var sqlResult = Convert.DataTo<SQLOperationResult<string>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_AddFile2Doc",
                Data = new AddFile2DocumentRequest()
                {
                    DocId = docId,
                    FileName = Path.GetFileName(path),
                    FilePath = path
                }
            }, cancellationToken)).Data);

            if (sqlResult is null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка при прикреплении файла к инвест-документу {} в БД  {msg}", path, sqlResult?.Message);
                throw new InvalidOperationException("Ошибка при прикреплении файла к инвест-документу");
            }
        }
    }
}
