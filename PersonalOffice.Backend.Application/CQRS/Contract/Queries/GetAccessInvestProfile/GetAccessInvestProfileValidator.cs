using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetAccessInvestProfile
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAccessInvestProfileValidator : AbstractValidator<GetAccessInvestProfile>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetAccessInvestProfileValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
        }
    }
}
