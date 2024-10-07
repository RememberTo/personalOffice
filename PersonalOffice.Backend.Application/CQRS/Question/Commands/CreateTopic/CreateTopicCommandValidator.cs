using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Question.Commands.CreateTopic
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        /// <summary>
        /// Валидирование полей
        /// </summary>
        public CreateTopicCommandValidator()
        {
            RuleFor(cmd => cmd.Subject).NotEmpty().NotNull();
            RuleFor(cmd => cmd.Text).NotEmpty().NotNull();
        }
    }
}
