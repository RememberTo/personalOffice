using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetInvestProfile
{
    /// <summary>
    /// 
    /// </summary>
    public class GetInvestProfileQueryValidator : AbstractValidator<GetInvestProfileQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetInvestProfileQueryValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
        }
    }
}
