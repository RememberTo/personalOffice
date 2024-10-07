namespace PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetPayments
{
    /// <summary>
    /// Модель представления документов
    /// </summary>
    public class PaymentsInfoVm
    {
        /// <summary>
        /// Общее количество документов
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Информация о последней инвестиционной анкете
        /// </summary>
        public string? LastAgreedInvestmentQuestionnaireInfo { get; set; }
        /// <summary>
        /// Список документов
        /// </summary>
        public ICollection<PaymentVm> Docs { get; set; } = [];
    }
}
