namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTestById.Vm
{
    /// <summary>
    /// Модель представления теста
    /// </summary>
    public class TestVm
    {
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название теста
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Список вопросов из раздела SelfEseetm
        /// </summary>
        public IEnumerable<QuestionVm>? SelfEsteemQuestions { get; set; }
        /// <summary>
        /// Список вопросов из раздела Knowledge
        /// </summary>
        public IEnumerable<QuestionVm>? KnowledgeQuestions { get; set; }
    }
}