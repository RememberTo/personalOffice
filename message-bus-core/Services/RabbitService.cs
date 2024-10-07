using MessageBus.Base;
using MessageBus.Common;
using MessageBus.Data;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Domain.Entites.EventTypes;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace MessageBus.Services
{
    public class RabbitService : ITransportService, IDisposable
    {
        public event AsyncEventHandler<MessageEvent>? RecivedMessage;
        public event AsyncEventHandler<MessageEvent>? SubRecivedMessage;

        private readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);
        private readonly Dictionary<MessageContentType, string> _contentTypes = new() {
            { MessageContentType.JSON , "application/json" },
            { MessageContentType.Binary , "application/octet-stream" },
            { MessageContentType.XML,"application/xml"},
        };
        private readonly ConcurrentDictionary<string, TaskCompletionSource<Message>> messageCompletionSources = new();
        private readonly RabbitReceiver rabbitReceiver;
        private readonly RabbitSender rabbitSender;
        private readonly ILogger? _logger;

        public static Message GetResponseMessage<T>(Message message, T Data)
        {
            return new Message
            {
                ID = message.ID,
                Source = message.Destination,
                Destination = message.Source ?? throw new ArgumentException("Неизвестный отправитель", nameof(message)),
                Method = message.Method,
                AuthToken = message.AuthToken,
                ContentType = message.ContentType,
                Type = message.Type,
                Version = message.Version,

                Data = Data
            };
        }

        public RabbitService(ILoggerFactory loggerFactory, ConnectionData connectionData)
        {
            try
            {
                _logger = loggerFactory.CreateLogger<RabbitService>();

                _logger?.LogInformation("Начало инициализации сервиса \n\t\t\tПараметры: {que}" +
                    "\n\t\t\t\tSSL: {ssl} \n\t\t\t\tLogin: {login} \n\t\t\t\tVirtual Host: {vh}" +
                    "\n\t\t\t\tCервер: amqp://{dom}:{port}", connectionData.ReceivedQueue, connectionData.IsSSL, 
                    connectionData.UserName, connectionData.VirtualHost, connectionData.HostName, connectionData.Port);

                rabbitReceiver = new RabbitReceiver(loggerFactory, connectionData);
                rabbitSender = new RabbitSender(loggerFactory, connectionData);

                rabbitReceiver.MessageReceived += ReceivedMessageHandlerAsync;
                rabbitReceiver.SubMessageReceived += SubReceivedMessageHandlerAsync;

                _logger?.LogInformation("Сервис инициализирован");
            }
            catch (Exception ex)
            {
                _logger?.LogInformation("Произошла ошибка при инициализации: {exMSg}", ex.Message);
                throw;
            }
        }

        public RabbitService(ILoggerFactory loggerFactory, Action<ConnectionData> options)
        {
            try
            {
                _logger = loggerFactory.CreateLogger<RabbitService>();

                _logger?.LogInformation("Начало инициализации сервиса");

                var connectionData = new ConnectionData();
                options(connectionData);

                rabbitReceiver = new RabbitReceiver(loggerFactory, connectionData);
                rabbitSender = new RabbitSender(loggerFactory, connectionData);

                rabbitReceiver.MessageReceived += ReceivedMessageHandlerAsync;
                rabbitReceiver.SubMessageReceived += SubReceivedMessageHandlerAsync;

                _logger?.LogInformation("Сервис инициализирован");
            }
            catch (Exception ex)
            {
                _logger?.LogInformation("Произошла ошибка при инициализации: {exMSg}", ex.Message);
                throw;
            }

        }



        private async Task SubReceivedMessageHandlerAsync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var response = Encoding.UTF8.GetString(@event.Body.ToArray());
                var msg = await SerializeHelper.JsonDeserizlizeAsync<Message>(response);

                _logger?.LogTrace("Пришло сообщение от: {ser}", msg.Source);

                if (messageCompletionSources.TryGetValue(msg.ID, out var tcs))
                {
                    _logger?.LogTrace("Сообщение является ответом на отправленное сообщение: {id}", msg.ID);
                    tcs?.TrySetResult(msg);
                }
                else
                {
                    _logger?.LogTrace("Вызов события для сообщения: {id}", msg.ID);
                    SubRecivedMessage?.Invoke(sender, new MessageEvent { Message = msg, Properties = @event.BasicProperties });
                }
            }
            catch (ArgumentNullException)
            {
                _logger?.LogInformation("Не удалось декодировать массив байт {ID}", @event.BasicProperties.MessageId);
            }
            catch (InvalidOperationException)
            {
                _logger?.LogInformation("Не удалось десериализовать сообщение в тип {type}", typeof(Message));
                throw;
            }
            catch (Exception)
            {
                _logger?.LogInformation("Непредвиденная ошибка");
                throw;
            }
        }

        private async Task ReceivedMessageHandlerAsync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var response = Encoding.UTF8.GetString(@event.Body.ToArray());
                var msg = await SerializeHelper.JsonDeserizlizeAsync<Message>(response);

                _logger?.LogTrace("Пришло сообщение от: {ser}", msg.Source);

                if (messageCompletionSources.TryGetValue(msg.ID, out var tcs))
                {
                    _logger?.LogTrace("Сообщение является ответом на отправленное сообщение: {id}", msg.ID);
                    tcs?.TrySetResult(msg);
                }
                else
                {
                    _logger?.LogTrace("Вызов события для сообщения: {id}", msg.ID);
                    RecivedMessage?.Invoke(sender, new MessageEvent { Message = msg, Properties = @event.BasicProperties });
                }
            }
            catch (ArgumentNullException)
            {
                _logger?.LogInformation("Не удалось декодировать массив байт {ID}", @event.BasicProperties.MessageId);
            }
            catch (InvalidOperationException)
            {
                _logger?.LogInformation("Не удалось десериализовать сообщение в тип {type}", typeof(Message));
                throw;
            }
            catch (Exception) 
            {
                _logger?.LogInformation("Непредвиденная ошибка");
                throw;
            }
        }

        public async Task SendMessageAsync(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);
            _logger?.LogTrace("Отправка сообщения в микросервис: {ser} метод:{met}", message.Destination, message.Method);

            SetPropertiesMessage(message);

            var data = await SerializeHelper.JsonSerizlizeAsync(message);

            await rabbitSender.SendMessageAsync(message.Destination, Encoding.UTF8.GetBytes(data));
        }

        public async Task SendResponseAsync<T>(Message message, T Data)
        {
            var msg = GetResponseMessage(message, Data);

            await SendMessageAsync(msg);
        }

        public void SendMessage(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);
            _logger?.LogTrace("Отправка сообщения в микросервис: {ser} метод:{met} id сообщения {idmsg}", message.Destination, message.Method, message.ID);

            SetPropertiesMessage(message);

            var data = SerializeHelper.JsonSerizlize(message);

            rabbitSender.SendMessage(message.Destination, Encoding.UTF8.GetBytes(data));
        }

        public void SendResponse<T>(Message message, T Data)
        {
            var msg = GetResponseMessage(message, Data);

            SendMessage(msg);
        }

        public async Task<Message> RPCServiceAsync(Message message, CancellationToken cancellationToken = default)
        {
            return await RPCServiceAsync(message, timeout: default, cancellationToken: cancellationToken);
        }

        public async Task<Message> RPCServiceAsync(Message message, TimeSpan timeout = default)
        {
            return await RPCServiceAsync(message, timeout: timeout, cancellationToken: default);
        }

        public async Task<Message> RPCServiceAsync(Message message, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            timeout = timeout == default ? defaultTimeout : timeout;
            _logger?.LogTrace("Вызов удаленной процедуры в микросервисе: {ser} метод:{met}", message.Destination, message.Method);

            try
            {
                var taskCompletionSource = new TaskCompletionSource<Message>();
                messageCompletionSources.TryAdd(message.ID, taskCompletionSource);

                if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();

                await SendMessageAsync(message);

                var msg =  await WaitForMessageAsync(taskCompletionSource, timeout, cancellationToken).ConfigureAwait(false);

                _logger?.LogTrace("Ответ от микросервиса: {ser} получен", message.Destination);

                return msg;
            }
            catch (TimeoutException)
            {
                _logger?.LogTrace("Не удалось получить ответ от микросервиса за {timeout}. message ID {mID} микросервис {ser}", timeout, message.ID, message.Destination);
                throw;
            }
            catch (TaskCanceledException)
            {
                _logger?.LogTrace("Задача была отменена message ID {mID} микросервис {ser}", message.ID, message.Destination);
                throw;
            }
            finally
            {
                messageCompletionSources.Remove(message.ID, out _);
            }
        }

        private static Task<Message> WaitForMessageAsync(TaskCompletionSource<Message> task, TimeSpan timeout, CancellationToken token = default)
        {
            var resultTask = Task.WhenAny(task.Task, Task.Delay(timeout, token)).GetAwaiter().GetResult();
            //var indTask = Task.WaitAny([task.Task], timeout);

            if (resultTask == task.Task)
            {
                return task.Task;
            }
            else if (resultTask.IsCanceled)
            {
                throw new TaskCanceledException();
            }
            else
            {
                throw new TimeoutException($"Не удалось получить ответ от сервиса за {timeout}");
            }
        }

        private void SetPropertiesMessage(Message message)
        {
            rabbitSender.MessageProperties.ContentType = _contentTypes[message.ContentType];
            rabbitSender.MessageProperties.MessageId = message.ID;
        }

        public void Dispose()
        {
            rabbitReceiver.MessageReceived -= ReceivedMessageHandlerAsync;
            rabbitReceiver.SubMessageReceived -= SubReceivedMessageHandlerAsync;
            rabbitReceiver?.Dispose();
            rabbitSender?.Dispose();
        }
    }
}
