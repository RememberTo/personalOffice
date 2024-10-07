using FluentValidation;
using PersonalOffice.Backend.Application.CQRS.User.Commands.ChangePassword;

namespace PersonalOffice.Backend.Application.CQRS.Security.Commands.ChangePassword
{
    /// <summary>
    /// Валидация
    /// </summary>
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        /// <summary>
        /// Валидация
        /// </summary>
        public ChangePasswordCommandValidator()
        {
            RuleFor(o => o.OldPasswordHash).NotNull().NotEmpty();
            RuleFor(o => o.NewPasswordHash).NotNull().NotEmpty();
        }
    }
}
