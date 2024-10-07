using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Validation;
using PersonalOffice.Backend.Application.Services;
using PersonalOffice.Backend.Application.Services.Base;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Reflection;

namespace PersonalOffice.Backend.Application
{
    /// <summary>
    /// Статический класс, для методов расширения DI
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Добавление в DI слоя Application
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <returns></returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())); // Добавляем mediatR используя текущую сборку
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); // добавляем валидацию как transient для каждого вызова объекта
            services.AddServices();

            return services;
        }

        /// <summary>
        /// Добавление связанных сервисов
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ITestCFIService, TestCFIServiceXML>(provider =>
                new TestCFIServiceXML(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XML", "TestsCFI"),
                provider.GetRequiredService<ILogger<TestCFIServiceXML>>()));

            services.AddScoped<ICacheManager, CacheManager>();
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<IClsCryptoService, ClsCryptoService>(provider => 
                new ClsCryptoService("64b99320-dd3a-11ec-9d64-0242ac120002", "34743777217a25432a462d4a614e6452", "z2k4m5n7q8r9t3k4m6p"));

            services.AddTransient<IContractService, ContractService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IOneTimePassService, OneTimePassService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<INotificationService, NotificationService>();

            return services;
        }
    }
}
