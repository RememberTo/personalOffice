using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.SetStatusDocument
{
    /// <summary>
    /// 
    /// </summary>
    public class SetStatusDocumentCommandValidator : AbstractValidator<SetStatusDocumentCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public SetStatusDocumentCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.DocumentId).NotEmpty();
            RuleFor(x => x.Status).GreaterThan(-1).NotEmpty().WithMessage("Не установлен статус документа");
        }
    }
}
