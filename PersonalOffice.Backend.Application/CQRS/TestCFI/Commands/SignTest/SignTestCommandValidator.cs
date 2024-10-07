using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Commands.SignTest
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class SignTestCommandValidator : AbstractValidator<SignTestCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public SignTestCommandValidator()
        {
            RuleFor(x => x.TestId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
