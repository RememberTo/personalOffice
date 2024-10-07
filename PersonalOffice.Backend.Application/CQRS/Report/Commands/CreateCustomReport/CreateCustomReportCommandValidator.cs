using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Report.Commands.CreateCustomReport
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCustomReportCommandValidator : AbstractValidator<CreateCustomReportCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateCustomReportCommandValidator()
        {
            RuleFor(x => x.BeginDate)
                .Must((command, beginDate) => (command.EndDate - beginDate).TotalDays <= 100)
                .WithMessage("Слишком большой период для формирования отчёта. Период не должен составлять более 100 календарных дней.");
            RuleFor(x => x.BeginDate)
                .Must((command, beginDate) => command.EndDate >= beginDate)
                .WithMessage("Начальная дата больше конечной");
            RuleFor(x => x.EndDate)
                .Must((command, endDate) => endDate <= DateTime.Now)
                .WithMessage("Измените конец отчетного периода. На выбранную дату информация отсутствует");
        }
    }
}
