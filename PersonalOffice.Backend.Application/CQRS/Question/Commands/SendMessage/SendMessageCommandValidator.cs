using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Question.Commands.SendMessage
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        /// <summary>
        /// Валидатор
        /// </summary>
        public SendMessageCommandValidator()
        {
            RuleFor(smc => smc.Text).NotEqual(string.Empty).NotNull();
            RuleFor(smc => smc.TopicId).GreaterThan(0); //должен быть больше 0
        }
    }
}
