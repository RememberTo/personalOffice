using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetMessagesFromTopic
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class GetMessagesQueryValidator : AbstractValidator<GetMessagesQuery>
    {
        /// <summary>
        /// Валидатор
        /// </summary>
        public GetMessagesQueryValidator()
        {
            RuleFor(smc => smc.TopicId).GreaterThan(0);
        }
    }
}
