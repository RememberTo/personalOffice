using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDocByIdQueryValidator : AbstractValidator<GetDocByIdQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetDocByIdQueryValidator()
        {
            RuleFor(x => x.DocId).NotEmpty().NotNull();   
        }
    }
}
