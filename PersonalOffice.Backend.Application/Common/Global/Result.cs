using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.Common.Global
{
    /// <summary>
    /// Реализация возвращаемого типа со статусом
    /// </summary>
    /// <param name="status">статус</param>
    /// <param name="message">сообщение по умолчанию null</param>
    /// <param name="data"></param>
    public class Result(InternalStatus status,
                        string? message = null,
                        object? data = null) : IResult
    {
        /// <summary>
        /// Статус
        /// </summary>
        public InternalStatus Status { get; set; } = status;
        /// <summary>
        /// Сооббщение
        /// </summary>
        public string? Message { get; set; } = message;
        /// <summary>
        /// Возвращаемый набор данных
        /// </summary>
        public object? Data { get; set; } = data ?? "no data";
    }
}
