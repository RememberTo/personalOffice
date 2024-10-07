using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateArbitraryDocument
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateArbitraryDocumentCommandValidator : AbstractValidator<CreateArbitraryDocumentCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateArbitraryDocumentCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ContractId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().WithMessage("Отсутсвует название документа").NotNull();
            RuleFor(x => x.UploadFiles.Count).GreaterThan(0).WithMessage("Нет прикрепленных файлов!");
        }
    }
}
