namespace PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetContractIndicators
{
    /// <summary>
    /// Модель представления индикаторов
    /// </summary>
    public class IndicatorsVm
    {
        /// <summary>
        /// Свободные средства
        /// </summary>
        public decimal Free { get; set; } = 0;
        /// <summary>
        /// Профит портфеля
        /// </summary>
        public decimal ProfitLoss { get; set; } = 0;
        /// <summary>
        /// Вложено дс
        /// </summary>
        public decimal Security { get; set; } = 0;
        /// <summary>
        /// Общее количество дс
        /// </summary>
        public decimal Total { get; set; } = 0; 
        /// <summary>
        /// Явялется ли этот договор активным
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Номер документа
        /// </summary>
        public string? DocNum { get; set; }
    }
}
