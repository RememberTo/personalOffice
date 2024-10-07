using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify
{
    /// <summary>
    /// 
    /// </summary>
    public class GetNotifyQueryValidator : AbstractValidator<GetNotifyQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetNotifyQueryValidator()
        {
            RuleFor(x => x.PageConfig.PageSize).GreaterThan(-1);
            RuleFor(x => x.PageConfig.PageNumber).GreaterThan(-1);
        }
    }
}
