using MediatR;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Commands.SignTest
{
    /// <summary>
    /// Контракт на подпись теста
    /// </summary>
    public class SignTestCommand : IRequest<IResult>
    {
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public required int TestId { get; set; }
        /// <summary>
        /// Код для подпсиания теста
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required int UserId { get; set; }
        /// <summary>
        /// Список ответов
        /// </summary>
        public required IEnumerable<string> Answers { get; set; }
    }
}
