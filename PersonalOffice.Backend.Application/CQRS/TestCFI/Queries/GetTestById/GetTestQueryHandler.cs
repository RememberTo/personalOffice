using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.General;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTestById.Vm;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTestById
{
    internal class GetTestQueryHandler(ILogger<GetTestQueryHandler> logger, ITestCFIService testCFIService)
        : IRequestHandler<GetTestQuery, TestVm>
    {
        private readonly ILogger<GetTestQueryHandler> _logger = logger;
        private readonly ITestCFIService _testCFIService = testCFIService;

        public Task<TestVm> Handle(GetTestQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Запрос на получение данных о тесте {TestName}", request.TestId);

            var test = _testCFIService.GetTestById(request.TestId);
            var testVm = new TestVm()
            {
                Id = test.TestId,
                Name = test.FileName,
                SelfEsteemQuestions = [],
                KnowledgeQuestions = [],
            };

            testVm.SelfEsteemQuestions = test.SelfEsteem.Questions.Select((x, index) => new QuestionVm
            {
                NumberQuest = ++index,
                TextQuest = x.Content,
                TypeControl = x.Type,
                AnswerVariants = x.AnswerVariants.Select(av => new AnswerVariantVm(av.IdAnswer, av.Content ?? string.Empty))
            });

            testVm.KnowledgeQuestions = _testCFIService
                .GetRandomQuestions(test.Knowledge, 1, 1) // сложность 1 | количество вопросов 1
                .Concat(_testCFIService.GetRandomQuestions(test.Knowledge, 2, 2))
                .Concat(_testCFIService.GetRandomQuestions(test.Knowledge, 3, 1))
                .Select((x, index) => new QuestionVm
                {
                    NumberQuest = ++index,
                    TextQuest = x.Content,
                    TypeControl = x.Type,
                    AnswerVariants = x.AnswerVariants.Select(av => new AnswerVariantVm(av.IdAnswer, av.Content ?? string.Empty))
                });

            _logger.LogTrace("Тест {TestId} сформирован количество SelfEseem {sfCnt} количество Knowledge {kCnt}",
                request.TestId, testVm.SelfEsteemQuestions.Count(), testVm.KnowledgeQuestions.Count());

            return Task.FromResult(testVm);
        }
    }
}


