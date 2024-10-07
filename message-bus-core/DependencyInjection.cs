using MessageBus.Data;
using MessageBus.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Domain.Interfaces.Services;
namespace MessageBus
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, Action<ConnectionData> options)
        {
            var connectionData = new ConnectionData();
            options(connectionData);

            services.AddSingleton<ITransportService, RabbitService>(provider =>
                new RabbitService(provider.GetRequiredService<ILoggerFactory>(), connectionData));

            return services;
        }

        public static IServiceCollection AddMessageBus(this IServiceCollection services, ConnectionData options)
        {
            services.AddSingleton<ITransportService, RabbitService>(provider =>
                new RabbitService(provider.GetRequiredService<ILoggerFactory>(), options));

            return services;
        }
    }
}
