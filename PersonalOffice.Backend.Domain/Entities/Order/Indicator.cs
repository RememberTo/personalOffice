namespace PersonalOffice.Backend.Domain.Entities.Order
{
    /// <summary>
    /// Модель индикатора
    /// </summary>
    public class Indicator
    {
        /// <summary>
        /// Наименование индикатора
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Код индикатора
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Номер поручения
        /// </summary>
        public required string Order { get; set; }
    }
}
