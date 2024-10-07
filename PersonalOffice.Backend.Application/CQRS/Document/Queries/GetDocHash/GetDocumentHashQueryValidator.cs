using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocHash
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDocumentHashQueryValidator : AbstractValidator<GetDocumentHashQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetDocumentHashQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.DocumentId).NotEmpty();
        }
    }
}
