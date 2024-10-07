using MessageBus.Data;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBus.Base
{
    internal class RabbitReceiver : RabbitMqConnection
    {
        public event AsyncEventHandler<BasicDeliverEventArgs>? MessageReceived;
        public event AsyncEventHandler<BasicDeliverEventArgs>? SubMessageReceived;

        public RabbitReceiver(ILoggerFactory loggerFactory, ConnectionData connectionData)
            : base(loggerFactory.CreateLogger<RabbitReceiver>(), connectionData)
        {
            ArgumentNullException.ThrowIfNull(nameof(Channel), "Не удалось создать подключение");

            try
            {
                if (ConnectionData.ReceivedQueue is not null)
                {
                    if (ConnectionData.ReceivedQueue.IsAutoCreatedQueue && !string.IsNullOrEmpty(ConnectionData.ReceivedQueue.QueueName))
                    {
                        AutoCreateQueue(ConnectionData.ReceivedQueue);
                    }

                    var consumerExclusive = new AsyncEventingBasicConsumer(Channel);
                    consumerExclusive.Received += RaiseEventAsync;

                    Channel.BasicConsume(
                        queue: ConnectionData.ReceivedQueue.QueueName,
                        autoAck: true,
                        consumer: consumerExclusive
                        );
                }
                if (ConnectionData.SubReceivedQueue is not null)
                {
                    if (ConnectionData.SubReceivedQueue.IsAutoCreatedQueue && !string.IsNullOrEmpty(ConnectionData.SubReceivedQueue.QueueName))
                    {
                        AutoCreateQueue(ConnectionData.SubReceivedQueue);
                    }

                    var consumerExclusive = new AsyncEventingBasicConsumer(Channel);
                    consumerExclusive.Received += ConsumerNoExclusive_Received;

                    Channel.BasicConsume(
                        queue: ConnectionData.SubReceivedQueue.QueueName,
                        autoAck: true,
                        consumer: consumerExclusive
                        );
                }
            }
            catch (Exception) { throw; }
        }

        private Task ConsumerNoExclusive_Received(object sender, BasicDeliverEventArgs @event)
        {
            SubMessageReceived?.Invoke(sender, @event);
            return Task.CompletedTask;
        }

        private Task RaiseEventAsync(object sender, BasicDeliverEventArgs @event)
        {
            MessageReceived?.Invoke(sender, @event);
            return Task.CompletedTask;
        }

        public void AckMessage(ulong Tag, bool multiple = false)
        {
            Channel.BasicAck(Tag, multiple);
        }
    }
}
