using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.NotifyFileQuery
{
    /// <summary>
    /// Валидация
    /// </summary>
    public class GetNotifyFileByIdQueryValidator : AbstractValidator<GetNotifyFileByIdQuery>
    {
        /// <summary>
        /// Валидация
        /// </summary>
        public GetNotifyFileByIdQueryValidator()
        {
            RuleFor(x => x.FileId).NotEmpty();
        }
    }
}
