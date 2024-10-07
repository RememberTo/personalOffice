namespace PersonalOffice.Backend.Domain.Entities.Contract
{
    /// <summary>
    /// Модель данных договора
    /// </summary>
    public class ContractDataResponse
    {
        /// <summary>
        /// Сумма договора
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Результат договора
        /// </summary>
        public decimal? ProfitLoss { get; set; }
    }
}
