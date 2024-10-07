using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractAllGraphs
{
    /// <summary>
    /// 
    /// </summary>
    public class GetContractAllGraphsQueryValidator : AbstractValidator<GetContractAllGraphsQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetContractAllGraphsQueryValidator()
        {
            RuleFor(x => x.BeginDate).NotEqual(default(DateTime));
            RuleFor(x => x.EndDate).NotEqual(default(DateTime));
        }
    }

}
