using System.Text.Json.Serialization;
using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.Domain.Entites.Transport
{
    /// <summary>
    /// Формат обмена данными
    /// </summary>
    [Serializable]
    public class Message
    {
        [JsonPropertyName("$type")]
        private string deserizlizeType => "MessageDataTypes.Message, MessageDataTypes";
#if DEBUG
        /// <summary>
        /// Название сервиса отправителя
        /// </summary>
        public string? SenderServiceName { get; set; }  //Сервис который отправил сообщение
#endif
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public string ID { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Тип сообщения (запрос или ответ)
        /// </summary>
        public MessageType Type { get; set; }
        /// <summary>
        /// Название вызываемого метода
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// Название очереди в которую будет отправлено сообщение
        /// </summary>
        public required string Destination { get; set; }
        /// <summary>
        /// Отправитель
        /// </summary>
        public string? Source { get; set; }
        /// <summary>
        /// Токен аутентификации
        /// </summary>
        public string? AuthToken { get; set; }
        /// <summary>
        /// Данные
        /// </summary>
        public required object Data { get; set; }
        /// <summary>
        /// Версия
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Тип содержимого сообщения
        /// </summary>
        [NonSerialized]
        public MessageContentType ContentType = MessageContentType.JSON;
    }
}
