using RabbitMQ.Client.Events;

namespace MessageBus.External
{
    public interface ITransportService : IDisposable
    {
        public event AsyncEventHandler<MessageEvent>? RecivedMessage;
        public event AsyncEventHandler<MessageEvent>? SubRecivedMessage;

        public Task<Message> RPCServiceAsync(Message message, TimeSpan timeout = default);
        public Task SendMessageAsync(Message message);
        public Task SendResponseAsync<T>(Message message, T Data);
        public void SendMessage(Message message);
        public void SendResponse<T>(Message message, T Data);
    }
}
