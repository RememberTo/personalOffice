using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.Application.Common.Exceptions;
using RabbitMQ.Client.Exceptions;

namespace PersonalOffice.Backend.API.Middlewares
{
    internal sealed class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status501NotImplemented,
                Type = "Unknown Failure https://tools.ietf.org/html/rfc9110#section-15.6.2",
                Title = "Необработанная ошибка",
                Detail = exception.Message
            };

            switch (exception)
            {
                case ValidationException ex:

                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Type = "Validation Failure https://tools.ietf.org/html/rfc9110#section-15.5.1";
                    problemDetails.Title = "Ошибка валидации данных";
                    problemDetails.Detail = exception.Message;
                    problemDetails.Extensions["errors"] = ex.Errors;

                    _logger.LogWarning("Ошибка при валидации данных, сообщение: {ex}, ошибки: {exErrs}", ex.Message, ex.Errors);

                    break;

                case NotFoundException ex:

                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Type = "Data not found https://tools.ietf.org/html/rfc9110#section-15.5.1";
                    problemDetails.Title = "Нет данных";
                    problemDetails.Detail = exception.Message;

                    _logger.LogWarning("Не найдены данные, сообщение: {ex}", exception.Message);

                    break;

                case TimeoutException ex:

                    problemDetails.Status = StatusCodes.Status408RequestTimeout;
                    problemDetails.Type = "Microservice Timeout https://tools.ietf.org/html/rfc9110#section-15.5.9";
                    problemDetails.Title = "Вышло время ожидания микросервиса";
                    problemDetails.Detail = exception.Message;

                    _logger.LogWarning("Вышло время ожидания микросервиса сообщение: {ex}", exception.Message);

                    break;

                case InvalidOperationException ex:

                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Type = "Microservice Failure https://tools.ietf.org/html/rfc9110#section-15.6.1";
                    problemDetails.Title = "Ошибка получения данных";
                    problemDetails.Detail = exception.Message;

                    _logger.LogError("Ошибка при обработке данных, сообщение: {ex}", exception.Message);

                    break;

                case BadHttpRequestException ex:

                    if (ex.StatusCode == 413)
                    {
                        problemDetails.Status = StatusCodes.Status413PayloadTooLarge;
                        problemDetails.Type = "Content Too Large https://tools.ietf.org/html/rfc9110#section-15.5.14";
                        problemDetails.Title = "Превышен лимит контента запроса";
                        problemDetails.Detail = exception.Message;

                        _logger.LogWarning("Превышен лимит контента запроса: {ex}", ex.Message);
                    }
                    else
                    {
                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Type = "Validation Failure https://tools.ietf.org/html/rfc9110#section-15.5.1";
                        problemDetails.Title = "Ошибка валидации данных";
                        problemDetails.Detail = exception.Message;

                        _logger.LogWarning("Ошибка при валидации данных, сообщение: {ex}", ex.Message);
                    }

                    break;

                case BadRequestException ex:

                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Type = "Request Failure https://tools.ietf.org/html/rfc9110#section-15.5.1";
                    problemDetails.Title = "Неверный запрос";
                    problemDetails.Detail = exception.Message;

                    _logger.LogWarning("Ошибка при запросе, сообщение: {ex}", ex.Message);

                    break;

                case ConnectFailureException:

                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Type = "Transport Failure https://tools.ietf.org/html/rfc9110#section-15.6.1";
                    problemDetails.Title = "Ошибка подключения";
                    problemDetails.Detail = exception.Message;

                    _logger.LogError("Ошибка RabbitMQ: {ex}", exception.Message);

                    break;

                case UnauthorizedAccessException:

                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Type = "Access Denied https://tools.ietf.org/html/rfc9110#section-15.5.4";
                    problemDetails.Title = "Доступ запрещен";
                    problemDetails.Detail = exception.Message;

                    _logger.LogWarning("Доступ запрещен: {ex}", exception.Message);

                    break;
            }

            context.Response.StatusCode = (int)problemDetails.Status;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
