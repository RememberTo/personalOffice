using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetQuestionsByAnswers
{
    /// <summary>
    /// Контракт на получение вопросов по ответам
    /// </summary>
    public class GetQuestionsByAnswersQuery : IRequest<IEnumerable<QuestionByAnswerVm>>
    {
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public required int TestId { get; set; }
        /// <summary>
        /// Список ответов
        /// </summary>
        public required IEnumerable<string> Answers { get; set; }
    }
}
