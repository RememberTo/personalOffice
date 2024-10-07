using Microsoft.AspNetCore.HttpLogging;

namespace PersonalOffice.Backend.API.Extensions
{
    /// <summary>
    /// Доплнительная логика обработки логирования для запросов
    /// </summary>
    public class RulesHttpLoggingInterceptor : IHttpLoggingInterceptor
    {
        private readonly List<string> _blockBodyLoggerForRequest = ["multipart/form-data"];

        /// <summary>
        /// Логер запроса
        /// </summary>
        /// <param name="logContext"></param>
        /// <returns></returns>
        public ValueTask OnRequestAsync(HttpLoggingInterceptorContext logContext)
        {
            var request = $"[{logContext.HttpContext.Request.Protocol}] [{logContext.HttpContext.Request.Method}] " +
                $"{logContext.HttpContext.Request.Scheme}://{logContext.HttpContext.Request.Host}{logContext.HttpContext.Request.Path}{logContext.HttpContext.Request.QueryString}";

            logContext.AddParameter(nameof(logContext.HttpContext.Request), $"{request}");

            return default;
        }

        /// <summary>
        /// Логер ответа
        /// </summary>
        /// <param name="logContext"></param>
        /// <returns></returns>
        public ValueTask OnResponseAsync(HttpLoggingInterceptorContext logContext)
        {
            var userId = logContext.HttpContext.User.FindFirst("UserId")?.Value;
            var user = string.IsNullOrEmpty(userId) ? "Пользователь без аутентификации" : userId;
            logContext.AddParameter("User", user);

            if (logContext.HttpContext.Request.Method != "GET" && 
                    _blockBodyLoggerForRequest.Any(x => logContext.HttpContext.Request.Headers.ContentType == x))
            {
                logContext.AddParameter("RequestBody", "file");
                logContext.LoggingFields = HttpLoggingFields.ResponseStatusCode |
                                           HttpLoggingFields.Duration |
                                           HttpLoggingFields.ResponseBody;
            }

            return default;
        }
    }
}
