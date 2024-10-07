using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetPayments
{
    /// <summary>
    /// 
    /// </summary>
    public class GetPaymentsQueryValidator : AbstractValidator<GetPaymentsQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetPaymentsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(-1);
            RuleFor(x => x.PageSize).GreaterThan(-1);
        }
    }
}
