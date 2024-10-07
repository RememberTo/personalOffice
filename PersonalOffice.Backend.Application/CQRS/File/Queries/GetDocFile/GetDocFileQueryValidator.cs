using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetDocFile
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDocFileQueryValidator : AbstractValidator<GetDocFileQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        public GetDocFileQueryValidator()
        {
            RuleFor(x => x.FileId).NotEmpty().NotNull();
        }
    }
}
