using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace MessageBusCore.Common
{
    public class LoggerFactory
    {
        public static ILoggerFactory CreateNLogLoggerFactory()
        {
            var logFactory = LogManager.Setup().LoadConfigurationFromFile("nlog.config");

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddNLog();
                })
                .BuildServiceProvider();

            return serviceProvider.GetRequiredService<ILoggerFactory>();
        }
    }
}
