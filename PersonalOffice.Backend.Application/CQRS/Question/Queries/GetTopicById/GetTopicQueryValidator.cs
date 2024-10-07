using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopicById
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class GetTopicQueryValidator : AbstractValidator<GetTopicByIdQuery>
    {
        /// <summary>
        /// Валидатор
        /// </summary>
        public GetTopicQueryValidator()
        {
            RuleFor(q => q.TopicId).GreaterThan(0);
        }
    }
}
