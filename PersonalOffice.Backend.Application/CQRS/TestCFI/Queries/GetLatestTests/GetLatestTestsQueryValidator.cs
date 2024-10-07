using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetLatestTests
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class GetLatestTestsQueryValidator : AbstractValidator<GetLatestTestsQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetLatestTestsQueryValidator()
        {
            RuleFor(x => x.Status).GreaterThan(-1).LessThan(2);
        }
    }
}
