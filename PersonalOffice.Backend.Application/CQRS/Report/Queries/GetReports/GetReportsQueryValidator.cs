using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Report.Queries.GetReports
{
    /// <summary>
    /// 
    /// </summary>
    public class GetReportsQueryValidator : AbstractValidator<GetReportsQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetReportsQueryValidator() 
        {
            RuleFor(x => x.Count).GreaterThan(-1);
            RuleFor(x => x.PageNum).GreaterThan(-1);
        }
    }
}
