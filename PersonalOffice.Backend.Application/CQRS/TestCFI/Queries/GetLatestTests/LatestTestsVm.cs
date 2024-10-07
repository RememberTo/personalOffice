namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetLatestTests
{
    /// <summary>
    /// Модель представления списка тестов
    /// </summary>
    public class LatestTestsVm
    {
        /// <summary>
        /// Количество страниц
        /// </summary>
        public int? PageCount { get; set; }
        /// <summary>
        /// Список последний пройденных тестов
        /// </summary>
        public IEnumerable<TestInfoVm>? Tests { get; set; }
    }
}
