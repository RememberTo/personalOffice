namespace PersonalOffice.Backend.Application.CQRS.Graph.Queries.General
{
    /// <summary>
    /// Представляет точку для графика
    /// </summary>
    public class PointGraphVm
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }
    }
}
