using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.General;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetQuestionsByAnswers
{
    internal class GetQuestionsByAnswersQueryHandler(ITestCFIService testCFIService, ILogger<GetQuestionsByAnswersQueryHandler> logger)
        : IRequestHandler<GetQuestionsByAnswersQuery, IEnumerable<QuestionByAnswerVm>>
    {
        private readonly ITestCFIService _testCFIService = testCFIService;
        private readonly ILogger<GetQuestionsByAnswersQueryHandler> _logger = logger;

        public Task<IEnumerable<QuestionByAnswerVm>> Handle(GetQuestionsByAnswersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Запрос на получение вопросов по id ответа. Название теста: {testName}, идентификаторы ответов: {idAnswers}",
                request.TestId, string.Join(";", request.Answers.ToArray()));

            var test = _testCFIService.GetTestById(request.TestId);

            try
            {
                var resultSelfEsteem = test.SelfEsteem.Questions
                   .SelectMany(quest => quest.AnswerVariants, (quest, answ) => new { quest, answ }) // выбираем вопрос, и список ответов на этот вопрос
                   .Where(qa => qa.answ != null && request.Answers.Contains(qa.answ.IdAnswer)) // отбираем только те варианты отвтеов, которые содержаится в запросе request
                   .GroupBy(x => x.answ.IdAnswer[..x.answ.IdAnswer.LastIndexOf('.')])//группируем по id так как на 1 вопрос может быть несколько ответов
                   .Select(g => new QuestionByAnswerVm // формируем объект ответа
                   {
                       TypeControl = g.First().quest.Type,
                       TextQuery = g.First().quest.Content,
                       Answers = g.Select(an => new AnswerVariantVm(an.answ.IdAnswer, an.answ.Content ?? string.Empty))
                   });

                var resultKnoweledge = test.Knowledge.Questions
                    .SelectMany(quest => quest.AnswerVariants, (quest, answ) => new { quest, answ })
                    .Where(qa => qa.answ != null && request.Answers.Contains(qa.answ.IdAnswer))
                    .GroupBy(x => x.answ.IdAnswer[..x.answ.IdAnswer.LastIndexOf('.')])
                    .Select(g => new QuestionByAnswerVm
                    {
                        TypeControl = g.First().quest.Type,
                        TextQuery = g.First().quest.Content,
                        Answers = g.Select(an => new AnswerVariantVm(an.answ.IdAnswer, an.answ.Content ?? string.Empty))
                    });

                _logger.LogTrace("Ответ сформирвоан:");

                return Task.FromResult(resultSelfEsteem.Concat(resultKnoweledge));
            }
            catch (Exception e)
            {
                _logger.LogTrace("Ошибка при формировании запроса: {emsg}", e.Message);
                return Task.FromResult(Enumerable.Empty<QuestionByAnswerVm>());
            }
        }
    }
}
