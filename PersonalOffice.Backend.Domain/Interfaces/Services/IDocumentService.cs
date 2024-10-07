using PersonalOffice.Backend.Domain.Entites.Document;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Entities.Document.Elements;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с документами
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Получение информации о документе
        /// </summary>
        /// <param name="documentInfo">Идентификатор документа и пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="isCached">Возвращать ли кеширвоанный ответ</param>
        /// <returns></returns>
        public Task<DocumentInfoDataTable> GetDocumentInfoAsync(DocumentInfoRequest documentInfo, CancellationToken cancellationToken = default, bool isCached = true);

        /// <summary>
        /// Добавляет документ во внутреннюю систему
        /// </summary>
        /// <param name="request">Параметры документа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task<int> AddDocumentAsync(AddDocumentRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Подписание произвольного документа
        /// </summary>
        /// <param name="signInfo">Данные для подпсиания документа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task SignArbitraryDocumentAsync(DocumentSignInfo signInfo, CancellationToken cancellationToken = default);
        /// <summary>
        /// Подписание любого типа документа
        /// </summary>
        /// <param name="docType">Тип документа</param>
        /// <param name="documentInfo">Информация о подписании документа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task SignDocumentAsync(int docType, DocumentSignInfo documentInfo, CancellationToken cancellationToken = default);
        /// <summary>
        /// Подписание инвест профиля
        /// </summary>
        /// <param name="signInfo">Информация о подписании документа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task SignInvestProfileDocumentAsync(DocumentSignInfo signInfo, CancellationToken cancellationToken = default);
        /// <summary>
        /// Подписание инвест справки
        /// </summary>
        /// <param name="signInfo">Информация о подписании документа</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task SignInvestSpravkaDocumentAsync(DocumentSignInfo signInfo, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathCommand"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetFilePathDocumentAsync(DocumentFilePathRequest pathCommand, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение содержимого файла
        /// </summary>
        /// <param name="formId">Идентификтор формы</param>
        /// <param name="Method">Метод report master'a</param>
        /// <returns></returns>
        public Task<byte[]> GetContentReportDocumentAsync(int formId, string Method);
        /// <summary>
        /// Добавление файла
        /// </summary>
        /// <param name="docId">Идентификатор документа</param>
        /// <param name="path">Путь к документу</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task AddFile2DocumentAsync(int docId, string path, CancellationToken cancellationToken = default);
        /// <summary>
        /// Устанавливает статус документа
        /// </summary>
        /// <param name="docId">Идентификатор документа</param>
        /// <param name="statusId">Идентификатор статуса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public Task SetStatusFormAsync(int docId, int statusId, CancellationToken cancellationToken = default);
    }
}
