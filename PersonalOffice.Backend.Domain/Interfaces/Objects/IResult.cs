using PersonalOffice.Backend.Domain.Common.Enums;

namespace PersonalOffice.Backend.Domain.Interfaces.Objects
{
    /// <summary>
    /// Формат представления различных статусов
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Статус запроса
        /// </summary>
        public InternalStatus Status { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public object? Data { get; set; }
    }
}
