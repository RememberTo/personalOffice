using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTestById
{
    /// <summary>
    /// Валидатор
    /// </summary>
    public class GetTestQueryValidator : AbstractValidator<GetTestQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetTestQueryValidator()
        {
            RuleFor(x => x.TestId).NotNull().NotEmpty().GreaterThan(0); ;
        }
    }
}
