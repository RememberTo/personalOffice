using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractGraph
{
    /// <summary>
    /// 
    /// </summary>
    public class GetContractGraphQueryValidator : AbstractValidator<GetContractGraphQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetContractGraphQueryValidator()
        {
            RuleFor(x => x.BeginDate).NotEqual(default(DateTime));
            RuleFor(x => x.EndDate).NotEqual(default(DateTime));
        }
    }

}
