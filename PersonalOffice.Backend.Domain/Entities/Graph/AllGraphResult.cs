namespace PersonalOffice.Backend.Domain.Entities.Graph
{
    /// <summary>
    /// Все значения графика
    /// </summary>
    public class AllGraphResult
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Стоимость портфеля
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Профит портфеля
        /// </summary>
        public decimal Profit { get; set; }
        /// <summary>
        /// Свободные средства
        /// </summary>
        public decimal Free { get; set; }
        /// <summary>
        /// Вложено в цб или PortfolioCost - AvailableFunds
        /// </summary>
        public decimal Sec { get; set; }
    }
}
