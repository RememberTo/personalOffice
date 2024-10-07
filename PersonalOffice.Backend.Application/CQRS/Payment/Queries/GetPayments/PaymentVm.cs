namespace PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetPayments
{
    /// <summary>
    /// Модель представления оплаты
    /// </summary>
    public class PaymentVm
    {
        /// <summary>
        /// Время регистрации
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// Номер договора
        /// </summary>
        public required string DocNum { get; set; }
        /// <summary>
        /// Название портфеля
        /// </summary>
        public required string TradeAccount { get; set; }
        /// <summary>
        /// Сумма пополнения
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }
}
