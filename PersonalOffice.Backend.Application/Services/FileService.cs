using Microsoft.Extensions.Logging;
using NLog;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.Services.Base;
using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entites.TestCFI;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace PersonalOffice.Backend.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    internal class FileService(IService<FileService> service) : IFileService
    {
        private readonly ITransportService _transportService = service.TransportService;
        private readonly ILogger<FileService> _logger = service.Logger;

        /// <summary>
        /// Сохраняет файл
        /// </summary>
        /// <param name="data">Параметры сохранения файла</param>
        /// <returns></returns>
        /// <exception cref="FileException">Если файл не удалось сохранить</exception>
        public async Task WriteFileAsync(FileOperationParameters data)
        {
            ArgumentNullException.ThrowIfNull(nameof(data));

            _logger.LogTrace("Начало сохранения файла {fname}", data.Name);

            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.FileManager,
                Method = "WriteFile",
                Data = data
            }, TimeSpan.FromSeconds(30));

            var fileResult = Common.Global.Convert.DataTo<FileOperationResult>(msg.Data);

            if (!fileResult.Success)
                throw new FileException(fileResult.Comment ?? "Неудачная попытка сохранить файл");

            _logger.LogTrace("Файл {fname} сохранен", data.Name);
        }

        /// <summary>
        /// Получение содержимого файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<FileOperationResult> GetFileAsync(string filePath, CancellationToken cancellationToken)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.FileManager,
                Method = "ReadFile",
                Data = new FileOperationParameters { Name = filePath },
            }, cancellationToken);

            var file = Common.Global.Convert.DataTo<FileOperationResult>(msg.Data);

            if (file == null || !file.Success)
                throw new InvalidOperationException("Ошибка чтения файла: " + file?.Comment);

            return file;
        }

        public async Task DeleteAsync(string filePath)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.FileManager,
                Method = "DeleteFile",
                Data = new FileOperationParameters { Name = filePath },
            }, TimeSpan.FromSeconds(15));

            //var file = Common.Global.Convert.DataTo<FileOperationResult>(msg.Data);

            //if (file == null || !file.Success)
            //    throw new InvalidOperationException("Ошибка удаления файла: " + file?.Comment);
        }

        public async Task<string> GetResourcesFileContent(string filename)
        {
            var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", filename);
            using var fs = new FileStream(filepath, FileMode.Open);

            var buffer = new byte[fs.Length];
            await fs.ReadAsync(buffer);

            return Encoding.Default.GetString(buffer);
        }
    }
}
