using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.SignDocument
{
    /// <summary>
    /// 
    /// </summary>
    public class SmsSignDocumentCommandValidator : AbstractValidator<SignDocumentCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public SmsSignDocumentCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.HashCertificate) || !string.IsNullOrEmpty(x.SmsCode))
                .WithMessage("Отсутствует данные для подписания документа");
        }
    }
}
