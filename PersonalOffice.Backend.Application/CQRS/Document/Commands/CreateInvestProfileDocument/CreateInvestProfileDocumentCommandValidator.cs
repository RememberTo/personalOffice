using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateInvestProfileDocument
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateInvestProfileDocumentCommandValidator : AbstractValidator<CreateInvestProfileDocumentCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateInvestProfileDocumentCommandValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
            RuleFor(x => x.Params).NotEmpty().WithMessage("Параметры обязательны для создания документа");
        }
    }
}
