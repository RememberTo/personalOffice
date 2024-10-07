namespace PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetVariantPayment
{
    /// <summary>
    /// Модель представления вариантов оплаты
    /// </summary>
    public class PaymentOptionsVm
    {
        /// <summary>
        /// Идентификатор оплаты, используется в пополнении
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Навзание операции
        /// </summary>
        public required string Name { get; set; }
    }
}
