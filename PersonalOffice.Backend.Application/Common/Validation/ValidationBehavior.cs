using FluentValidation;
using MediatR;

namespace PersonalOffice.Backend.Application.Common.Validation
{
    /// <summary>
    /// Валидация типов
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    /// <param name="validators">список правил валидации</param>
    internal class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        /// <summary>
        /// Собирает все ошибки валидации и генерирует исключение
        /// </summary>
        /// <param name="request">Тип запрос</param>
        /// <param name="next">Делегат следующего обработчика</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">Ошибка валидации</exception>
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
