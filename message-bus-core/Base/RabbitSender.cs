using MessageBus.Data;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MessageBus.Base
{
    internal class RabbitSender : RabbitMqConnection
    {
        private const string EXCHANGE = "SolidMain";
        public IBasicProperties MessageProperties { get; set; }
        public RabbitSender(ILoggerFactory loggerFactory, ConnectionData connectionData)
            : base(loggerFactory.CreateLogger<RabbitSender>(), connectionData)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(nameof(Channel), "Не удалось создать подключение");

                MessageProperties = Channel.CreateBasicProperties();
            }
            catch (Exception) { throw; }
        }

        public async Task SendMessageAsync(string queueName, byte[] body)
        {
            await Task.Run(() => PublishMessage(EXCHANGE, queueName, body));
        }

        public void SendMessage(string queueName, byte[] body)
        {
            PublishMessage(EXCHANGE, queueName, body);
        }

        private void PublishMessage(string exchange, string routingKey, byte[] body)
        {
            Channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: MessageProperties, body: body);
        }
    }
}
