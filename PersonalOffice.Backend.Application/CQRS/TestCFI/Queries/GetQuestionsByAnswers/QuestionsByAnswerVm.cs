using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.General;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetQuestionsByAnswers
{
    /// <summary>
    /// Модель представления вопросов
    /// </summary>
    public class QuestionByAnswerVm
    {
        /// <summary>
        /// Тип контрола HTML
        /// </summary>
        public string? TypeControl { get; set; }
        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string? TextQuery { get; set; }
        /// <summary>
        /// Список ответов
        /// </summary>
        public IEnumerable<AnswerVariantVm>? Answers { get; set; }
    }
}
