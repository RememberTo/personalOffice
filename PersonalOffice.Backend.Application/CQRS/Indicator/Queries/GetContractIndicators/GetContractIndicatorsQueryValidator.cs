using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetContractIndicators
{
    /// <summary>
    /// 
    /// </summary>
    public class GetContractIndicatorsQueryValidator : AbstractValidator<GetContractIndicatorsQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetContractIndicatorsQueryValidator()
        {
            RuleFor(x => x.BeginDate).NotEqual(default(DateTime));
            RuleFor(x => x.EndDate).NotEqual(default(DateTime));
        }
    }
}
