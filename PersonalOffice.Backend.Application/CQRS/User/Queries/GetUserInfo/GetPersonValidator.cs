using FluentValidation;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetMessagesFromTopic;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserInfo
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class GetUserInfoQueryValidator : AbstractValidator<GetMessagesQuery>
    {
        /// <summary>
        /// Валидатор
        /// </summary>
        public GetUserInfoQueryValidator()
        {
            RuleFor(smc => smc.TopicId).GreaterThan(0);
        }
    }
}
