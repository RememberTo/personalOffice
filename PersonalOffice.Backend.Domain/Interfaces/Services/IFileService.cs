using PersonalOffice.Backend.Domain.Entites.File;
using System.Text;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с файлами
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Сохраняет файлы во внутренней системе
        /// </summary>
        /// <param name="parameters">Параметры сохранения файла</param>
        /// <returns></returns>
        public Task WriteFileAsync(FileOperationParameters parameters);
        /// <summary>
        /// Получение содержимого файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Если не удалось получить файл</exception>
        public Task<FileOperationResult> GetFileAsync(string filePath, CancellationToken cancellationToken);

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Если не удалось удалить файл</exception>
        public Task DeleteAsync(string filePath);

        /// <summary>
        /// Возвращает контент файла из папки Domain/Resources
        /// </summary>
        /// <param name="filename">Путь к файлу из папки Domain/Resources</param>
        /// <returns></returns>
        public Task<string> GetResourcesFileContent(string filename);
    }
}
