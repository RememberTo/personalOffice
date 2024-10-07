using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetCustomReportFile
{
    /// <summary>
    /// Валидация
    /// </summary>
    public class GetReportFileQueryValidator : AbstractValidator<GetCustomReportFileQuery>
    {
        /// <summary>
        /// Валидация
        /// </summary>
        public GetReportFileQueryValidator()
        {
            RuleFor(x => x.ReportId).NotEmpty().NotNull();
            RuleFor(x =>x.UserId).NotEmpty();   
        }
    }
}
