using FluentValidation;
using PersonalOffice.Backend.Application.CQRS.File.Queries.GetReportFile;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetReportFile
{
    /// <summary>
    /// Валидация
    /// </summary>
    public class GetReportFileQueryValidator : AbstractValidator<GetReportFileQuery>
    {
        /// <summary>
        /// Валидация
        /// </summary>
        public GetReportFileQueryValidator()
        {
            RuleFor(x => x.FileId).NotEmpty().NotNull();
            RuleFor(x =>x.IsSignFile).NotNull();   
        }
    }
}
