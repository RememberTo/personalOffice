using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocs
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDocsQueryValidator : AbstractValidator<GetDocsQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetDocsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(-1);
            RuleFor(x =>x.Page).GreaterThan(-1);
        }
    }
}
