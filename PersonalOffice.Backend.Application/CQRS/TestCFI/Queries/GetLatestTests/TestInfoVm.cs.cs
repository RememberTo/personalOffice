namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetLatestTests
{
    /// <summary>
    /// Модель представления теста
    /// </summary>
    public class TestInfoVm
    {
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Номер теста
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// Дата прохождения
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Название теста
        /// </summary>
        public string? TestName { get; set; }
        /// <summary>
        /// Название теста в родительном падеже
        /// </summary>
        public string? TestNameGenitiveCase { get; set; }
        /// <summary>
        /// Статус теста пройден или нет
        /// </summary>
        public string? Status { get; set; }
    }
}
