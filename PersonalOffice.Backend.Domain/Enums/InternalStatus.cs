using System.Text.Json.Serialization;

namespace PersonalOffice.Backend.Domain.Common.Enums
{
    /// <summary>
    /// Внутренний статус команд и запросов
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))] // конвертирование статуса в строку
    public enum InternalStatus
    {
        /// <summary>
        /// Выполенно
        /// </summary>
        Success,
        /// <summary>
        /// Отправлено
        /// </summary>
        Sent,
        /// <summary>
        /// Доступ запрещен
        /// </summary>
        AccessDenied,
        /// <summary>
        /// Доступ разрешен
        /// </summary>
        AccessAllowed,
        /// <summary>
        /// Создано
        /// </summary>
        Created,
        /// <summary>
        /// Не найдено
        /// </summary>
        NotFound,
        /// <summary>
        /// Ошибка
        /// </summary>
        Error,
        /// <summary>
        /// Тест пройден
        /// </summary>
        TestPassed,
        /// <summary>
        /// Тест не пройден
        /// </summary>
        TestFailed,
    }
}
