using MessageBus.Data;
using MessageBusCore.Data;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Security;

namespace MessageBus.Base
{
    public abstract class RabbitMqConnection : IDisposable
    {
        private ConnectionFactory _factory;
        private readonly int _heartbeatInterval = 20;
        private readonly ILogger? _logger;

        protected IConnection Connection;
        protected IModel Channel;
        protected ConnectionData ConnectionData;

        public RabbitMqConnection(ILogger logger, ConnectionData connectionData)
        {
            ConnectionData = connectionData ??
                throw new ArgumentNullException(nameof(connectionData), "Нет данных для подключения");

            try
            {
                _logger = logger;
                Connect();
            }
            catch (Exception e)
            {
                _logger?.LogError("Ошибка при подклоючении {Error}", e.Message);
            }

            if (_factory is null) { throw new ConnectFailureException("Не удалось инициализировать данные", null); }
            if (Connection is null) { throw new ConnectFailureException("Не удалось создать подключение", null); }
            if (Channel is null) { throw new ConnectFailureException("Не удалось создать канал", null); }

            Connection.ConnectionShutdown += ConnectionShutdown;
        }

        private void ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger?.LogWarning("Произошел разрыв соединения! Инициатор{Iniziator} Сообщение: {ReplyText} Код: {ReplyCode}",
                e.Initiator.ToString(), e.ReplyText, e.ReplyCode);

            ReconnectWithRetry();
        }

        private void ReconnectWithRetry()
        {
            var maxRetryAttempts = 9;
            var retryDelaySeconds = 20;
            var retryCount = 0;

            while (retryCount < maxRetryAttempts)
            {
                try
                {
                    Connect();
                    _logger?.LogInformation("Соединение восстановлено");
                    return;
                }
                catch (Exception ex)
                {
                    _logger?.LogInformation("Неудачная попоытка реконнекта: {exMessage}. Повтор через {retryDelaySeconds} секунд...", ex.Message, retryDelaySeconds);
                    Thread.Sleep(retryDelaySeconds * 1000);
                    retryCount++;
                }
            }

            _logger?.LogError("Не удалось переподключится после {maxRetryAttempts} попыток, остановка переподключения...", maxRetryAttempts);
        }

        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }

        protected void Connect()
        {
            try
            {
                _factory = new ConnectionFactory
                {
                    HostName = ConnectionData.HostName,
                    Port = ConnectionData.Port,
                    UserName = ConnectionData.UserName,
                    Password = ConnectionData.Password,
                    VirtualHost = ConnectionData.VirtualHost,
                    RequestedHeartbeat = TimeSpan.FromSeconds(_heartbeatInterval),
                    DispatchConsumersAsync = true,
                    AutomaticRecoveryEnabled = true,
                    TopologyRecoveryEnabled = true,
                    //AmqpUriSslProtocols = System.Security.Authentication.SslProtocols.Default
                };

                if (ConnectionData.IsSSL)
                {
                    _factory.Ssl.Enabled = true;
                    //_factory.Ssl = new SslOption
                    //{
                    //    Enabled = true,
                    //    CertPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certs", "Development","cacert.pem"),
                    //};
                    //_logger?.LogInformation("Путь к сертификату {pth}", _factory.Ssl.CertPath);
                    _factory.Ssl.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors | SslPolicyErrors.RemoteCertificateNotAvailable;
                }

                Connection = _factory.CreateConnection();
                Channel = Connection.CreateModel();

            }
            catch (Exception) { throw; }
        }

        protected virtual void Disconnect()
        {
            if (Channel != null && Channel.IsOpen)
            {
                Channel.Close();
            }

            if (Connection != null && Connection.IsOpen)
            {
                Connection.Close();
            }
        }
        protected virtual bool AutoCreateQueue(ReceivedQueueData queueData)
        {
            try
            {
                _logger?.LogInformation("Создание очереди с параметрами {data}", queueData.ToString());

                var result = Channel.QueueDeclare(
                    queue: queueData.QueueName,
                    durable: true,
                    exclusive: queueData.Exclusive,
                    autoDelete: queueData.AutoDelete);

                if (queueData.BindQueue)
                {
                    Channel.QueueBind(
                    queue: queueData.QueueName,
                    exchange: queueData.Exchange,
                    routingKey: queueData.QueueName);
                }

                _logger?.LogInformation("Очередь {QName} успешно создана", result.QueueName);

                return result.QueueName == queueData.QueueName; //?????
            }
            catch (Exception)
            {

                throw new InvalidOperationException($"Не удалось создать очередь {queueData.QueueName}");
            }
        }

    }
}
