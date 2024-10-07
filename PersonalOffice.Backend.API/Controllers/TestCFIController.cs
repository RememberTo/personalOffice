using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.TestCFI;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Commands.SignTest;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetLatestTests;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetQuestionsByAnswers;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTestById;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTests;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с тестами для неквалифицированных инвесторов
    /// </summary>
    public class TestCFIController(IMapper mapper) : BaseController
    {
        private readonly IMapper _mapper = mapper;
        /// <summary>
        /// Получение информации о тестах
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/tests
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список тестов</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<TestVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("tests")]
        public async Task<ActionResult<IEnumerable<TestVm>>> GetTests(CancellationToken cancellationToken)
        {
            var tests = await Mediator.Send(new GetTestsQuery { UserID = base.UserId }, cancellationToken);

            return Ok(tests);
        }

        /// <summary>
        /// Получение теста по ID
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/tests/{testId}
        ///     
        /// </remarks>
        /// <param name="testId">ID теста</param>
        /// <returns></returns>
        /// <response code="200">Тест</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TestVm), StatusCodes.Status200OK)]
        [HttpGet("tests/{testId}")]
        public async Task<IActionResult> GetTestById([FromRoute] int testId)
        {
            var test = await Mediator.Send(new GetTestQuery { TestId = testId });
            return Ok(test);
        }

        /// <summary>
        /// Список недавно пройденных тестов
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/tests/recent?page=0pageSize=10
        ///     
        /// </remarks>
        /// <param name="status">рудимент</param>
        /// <param name="page">текущая страница начиная с 0</param>
        /// <param name="pageSize">количество элементов в ответе</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">Список недавно пройденных тестов</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LatestTestsVm), StatusCodes.Status200OK)]
        [HttpGet("tests/recent")]
        public async Task<IActionResult> GetLatestTests(
            [FromQuery] int status,
            [FromQuery] int page,
            [FromQuery] int? pageSize,
            CancellationToken cancellationToken)
        {
            pageSize ??= 10;
            var tests = await Mediator.Send(new GetLatestTestsQuery
            {
                UserID = UserId,
                Status = status,
                Page = page,
                PageSize = pageSize.Value
            }, cancellationToken);

            return Ok(tests);
        }

        /// <summary>
        /// Получение списка вопросв и ответов пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET tests/ЗПИФ/questions?answers=a1;a2;b1;b2;b3;b4;c1
        ///     
        /// </remarks>
        /// <param name="testId">Идентификатор теста</param>
        /// <param name="answers">список ответов пользователя указываются через ; одной строкой</param>
        /// <returns></returns>
        /// <response code="200">Список вопросов, для ответов</response>
        /// <response code="400">Ошибка API</response>
        [Obsolete]
        [ProducesResponseType(typeof(IEnumerable<QuestionByAnswerVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("tests/{testId}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionByAnswerVm>>> GetQuestionsForAnswers([FromRoute] int testId, [FromQuery] string answers)
        {
            var res = await Mediator.Send(new GetQuestionsByAnswersQuery { TestId = testId, Answers = answers.Split(';') });

            return Ok(res);
        }

        /// <summary>
        /// Подписание теста
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/tests/sign
        ///     {
        ///         testId: 1
        ///         code: 123456
        ///         answers: "a1;a2;a3;a4"
        ///     }
        ///     
        /// </remarks>
        /// <param name="testId">Идентфиикатор теста</param>
        /// <param name="signTestDto"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Domain.Interfaces.Objects.IResult), StatusCodes.Status200OK)]
        [HttpPost("tests/{testId}/sign")]
        public async Task<IActionResult> SignTest([FromRoute] int testId, [FromBody] SignTestDto signTestDto)
        {
            var mapDto = _mapper.Map<SignTestCommand>(signTestDto);
            mapDto.UserId = base.UserId;
            mapDto.TestId = testId;
            var result = await Mediator.Send(mapDto);

            return Ok(result);
        }
    }
}
