using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetGeneralIndicators
{
    /// <summary>
    /// 
    /// </summary>
    public class GetGeneralIndicatorsQueryValidator : AbstractValidator<GetGeneralIndicatorsQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetGeneralIndicatorsQueryValidator()
        {
            RuleFor(x => x.BeginDate).NotEqual(default(DateTime));
            RuleFor(x => x.EndDate).NotEqual(default(DateTime));
        }
    }
}
