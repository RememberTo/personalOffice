using PersonalOffice.Backend.API.Middlewares;

namespace PersonalOffice.Backend.API.Extensions
{
    /// <summary>
    /// Класс позволяющий внедрять в DI контейнер, различные middleware
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Внедрение ExcetionMiddleware для обработки ошибок
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
