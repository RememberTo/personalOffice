using RabbitMQ.Client;

namespace MessageBus.External
{
    public class MessageEvent : EventArgs
    {
        public required Message Message {  get; set; }
        public required IBasicProperties Properties { get; set; }
    }
}
