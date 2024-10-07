using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.Services.Base
{
    /// <summary>
    /// Базовый интерфейс сервиса
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IService<TService>
    {
        internal ILogger<TService> Logger { get; set; }
        internal ITransportService TransportService { get; set; }
    }

    /// <summary>
    /// Базовая реализация сервиса
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="logger">Логгирвоание сервиса</param>
    /// <param name="transportService">Основной транспортный шлюз для обмена данными</param>
    public class Service<TService>(
        ILogger<TService> logger,
        ITransportService transportService) : IService<TService>
    {
        ILogger<TService> IService<TService>.Logger { get; set; } = logger;
        ITransportService IService<TService>.TransportService { get; set; } = transportService;
    }
}
