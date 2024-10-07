using PersonalOffice.Backend.Domain.Entites.EventTypes;
using PersonalOffice.Backend.Domain.Entites.Transport;
using RabbitMQ.Client.Events;

namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Интерфейс для отправки и получения данных
    /// </summary>
    public interface ITransportService : IDisposable
    {
        /// <summary>
        /// Событие получение сообщений из очереди
        /// </summary>
        public event AsyncEventHandler<MessageEvent>? RecivedMessage;
        /// <summary>
        /// Второе событие получение сообщений из очереди по необходимости
        /// </summary>
        public event AsyncEventHandler<MessageEvent>? SubRecivedMessage;

        /// <summary>
        /// (async) Отправляет сообщение и ожидает ответ
        /// </summary>
        /// <param name="message">Сообщение для отправки</param>
        /// <param name="timeout">Время ожидания ответа от микросервиса</param>
        /// <returns></returns>
        public Task<Message> RPCServiceAsync(Message message, TimeSpan timeout = default);
        /// <summary>
        /// (async) Отправляет сообщение и ожидает ответ
        /// </summary>
        /// <param name="message">Сообщение для отправки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        public Task<Message> RPCServiceAsync(Message message, CancellationToken cancellationToken = default);
        /// <summary>
        /// (async) Отправка сообщения 
        /// </summary>
        /// <param name="message">Отправляемое сообщение</param>
        /// <returns></returns>
        public Task SendMessageAsync(Message message);
        /// <summary>
        /// (async) Отправка сообщения 
        /// </summary>
        /// <param name="message">Отправляемое сообщение</param>
        /// <param name="Data">Данные отправляемые в сообщении</param>
        /// <returns></returns>
        public Task SendResponseAsync<T>(Message message, T Data);
        /// <summary>
        /// Отправка сообщения 
        /// </summary>
        /// <param name="message">Отправляемое сообщение</param>
        /// <returns></returns>
        public void SendMessage(Message message);
        /// <summary>
        /// Отправка ответа на сообщение
        /// </summary>
        /// <param name="message">Сообщение которое пришло от микросервиса</param>
        /// <param name="Data">Данные отправляемые в сообщении</param>
        /// <returns></returns>
        public void SendResponse<T>(Message message, T Data);
    }
}
