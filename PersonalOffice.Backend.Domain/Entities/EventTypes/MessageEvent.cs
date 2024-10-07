using PersonalOffice.Backend.Domain.Entites.Transport;
using RabbitMQ.Client;

namespace PersonalOffice.Backend.Domain.Entites.EventTypes
{
    /// <summary>
    /// Тип события
    /// </summary>
    public class MessageEvent : EventArgs
    {
        /// <summary>
        /// Сообщение которое пришло в очередь
        /// </summary>
        public required Message Message { get; set; }
        /// <summary>
        /// Настройки
        /// </summary>
        public required IBasicProperties Properties { get; set; }
    }
}
