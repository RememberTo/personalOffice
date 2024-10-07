using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopics
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class GetTopicsQueryValidator : AbstractValidator<GetTopicsQuery>
    {
        /// <summary>
        /// Валидатор
        /// </summary>
        public GetTopicsQueryValidator()
        {
            RuleFor(topic => topic.UserId).GreaterThan(0);
            RuleFor(topic => topic.MaxTopics).GreaterThan(-1); // при 0 в процедуре БД береться значение 9999999
        }
    }
}
