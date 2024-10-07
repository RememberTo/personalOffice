using FluentValidation;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Commands.TopUpAccount
{
    /// <summary>
    /// 
    /// </summary>
    public class TopUpAccountCommandValidator : AbstractValidator<TopUpAccountCommand>
    {
        /// <summary>
        ///
        /// </summary>
        public TopUpAccountCommandValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0)
                .WithMessage("Ошибка: сумма перевода должна быть положительной");
            RuleFor(x => x.PortfolioId).Must((command, portfolioId) => portfolioId > 0)
                .WithMessage("Ошибка: не выбрано назначение платежа.");
            RuleFor(x => x.Quantity).Must((command, quantity) => quantity < 600_000)
                .WithMessage("Ошибка: сумма перевода должна быть меньше 600 000 руб.");

        }
    }
}
