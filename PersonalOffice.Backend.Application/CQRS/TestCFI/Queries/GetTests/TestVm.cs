namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTests
{
    /// <summary>
    /// Модель предсивления информации о тесте
    /// </summary>
    public class TestVm
    {
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Название теста
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Навзание теста в родительном падеже
        /// </summary>
        public string? NameGenitiveCase { get; set; }
        /// <summary>
        /// Описание теста
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Идентификатор статуса
        /// </summary>
        public int IdStatus { get; set; }
        /// <summary>
        /// Статус пройден ли тест или нет
        /// </summary>
        public string? Status { get; set; }
    }
}
