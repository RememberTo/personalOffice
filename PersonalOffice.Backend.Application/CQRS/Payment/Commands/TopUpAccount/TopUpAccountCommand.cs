using MediatR;
using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Commands.TopUpAccount
{
    /// <summary>
    /// Контракт на создание профиля пополнения
    /// </summary>
    public class TopUpAccountCommand : IRequest<TopUpVm>
    {
        /// <summary>
        /// Идентфиикатор пользователя
        /// </summary>
        public int UsertId { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int ContractId { get; set; }
        /// <summary>
        /// Идентификатор порфтеля
        /// </summary>
        public int PortfolioId { get; set; }
        /// <summary>
        /// Валюта пополнения
        /// </summary>
        public Currency Currency { get; set; } = Currency.RoubleRF;
        /// <summary>
        /// Тип пополнения
        /// </summary>
        public PaymentOptions PaymentOptions { get; set; }
        /// <summary>
        /// Сумма пополнения
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Оплата с мобильного
        /// </summary>
        public bool IsMobile { get; set; }
    }
}
