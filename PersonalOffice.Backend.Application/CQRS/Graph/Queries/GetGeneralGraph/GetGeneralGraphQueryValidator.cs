using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetGeneralGraph
{
    /// <summary>
    /// 
    /// </summary>
    public class GetGeneralGraphQueryValidator : AbstractValidator<GetGeneralGraphQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetGeneralGraphQueryValidator()
        {
            RuleFor(x => x.BeginDate).NotEqual(default(DateTime));
            RuleFor(x => x.EndDate).NotEqual(default(DateTime));
        }
    }

}
