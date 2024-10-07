using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Question;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Question.Commands.CreateTopic;
using PersonalOffice.Backend.Application.CQRS.Question.Commands.SendMessage;
using PersonalOffice.Backend.Application.CQRS.Question.General;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetMessagesFromTopic;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopicById;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopics;

namespace PersonalOffice.Backend.API.Controllers
{//ctrl+m+o or p
    /// <summary>
    /// Контроллер обмена сообщениями с менеджером пользоваителя ЛК
    /// </summary>
    /// <param name="logger">Логер</param>
    /// <param name="mapper">Маппинг данных</param>
    public class QuestionController(ILogger<QuestionController> logger, IMapper mapper) : BaseController
    {

        private readonly ILogger<QuestionController> _logger = logger;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Получает список диалогов с менеджером пользователя ЛК
        /// </summary>
        /// <remarks> 
        /// Пример запроса:
        /// 
        ///     GET /api/topics 3779
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <param name="TopicTypeCode">Код типа топика</param>
        /// <param name="MaxTopics">Максимальное количество топиков в ответе</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<TopicVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("topics")]
        public async Task<ActionResult<IEnumerable<TopicVm>>> GetTopics([FromQuery] int? MaxTopics, CancellationToken cancellationToken, [FromQuery] string? TopicTypeCode = null)
        {
            //переписать по аналогии с SignTestDto : IMapWith<SignTestCommand>
            //var query = _mapper.Map<GetTopicsQuery>(new dto { }}(base.UserId, TopicTypeCode));
            var query = _mapper.Map<GetTopicsQuery>((base.UserId, TopicTypeCode));

            query.MaxTopics = MaxTopics ?? query.MaxTopics;

            var topics = await Mediator.Send(query, cancellationToken);

            return Ok(topics);
        }

        /// <summary>
        /// Создание нового топика
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST /api/topics
        ///     {
        ///         "subject": "Акции",
        ///         "text": "Как купить акции"
        ///     }
        ///     
        /// </remarks>
        /// <param name="newTopic">Модель для создания топика</param>
        /// <response code="201">Топик создан</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("topics")]
        [RequestSizeLimit(50_000)] //50 килобайт
        public async Task<IActionResult> CreateTopic([FromBody] NewTopicDto newTopic)
        {
            if (!ModelState.IsValid) return BadRequest();

            var createTopic = _mapper.Map<CreateTopicCommand>(newTopic);
            
            createTopic.UserID = base.UserId;

            var result = await Mediator.Send(createTopic);

            return Created(nameof(NewTopicDto), result);
        }

        /// <summary>
        /// Информация о топике
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/topics/5315
        /// 
        /// </remarks>
        /// <param name="TopicId">Идентификатор топика</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(TopicInfoVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("topics/{TopicId}")]
        public async Task<ActionResult<TopicInfoVm>> GetTopicById([FromRoute]int TopicId, CancellationToken cancellationToken)
        {
            var topic = await Mediator.Send(new GetTopicByIdQuery { UserId = base.UserId, TopicId = TopicId }, cancellationToken);
            
            return Ok(topic);
        }

        /// <summary>
        /// Все сообщения для определенного топика
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/topics/5315/messages
        /// 
        /// </remarks>
        /// <param name="TopicId">Идентификатор топика</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<TopicMessageVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("topics/{TopicId}/messages")]
        public async Task<ActionResult<IEnumerable<TopicMessageVm>>> GetMessagesFromTopic(int TopicId, CancellationToken cancellationToken)
        {
            var messages = await Mediator.Send(new GetMessagesQuery { UserId = base.UserId, TopicId = TopicId }, cancellationToken);

            return Ok(messages);
        }

        /// <summary>
        /// Отправление сообщения в топик
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/topics/5315/messages
        ///     {
        ///         "text": "hello world!"
        ///     }
        ///     
        /// </remarks>
        /// <param name="TopicId">Идентификатор топика</param>
        /// <param name="textDto">Текст сообщения</param>
        /// <returns></returns>
        /// <response code="201">Сообщение отправлено</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("topics/{TopicId}/messages")]
        [RequestSizeLimit(50_000)] //50 килобайт
        public async Task<IActionResult> SendMessageForTopic(int TopicId, [FromBody] MessageTextDto textDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            var result = await Mediator.Send(new SendMessageCommand {UserId = base.UserId, TopicId = TopicId, Text = textDto.Text });

            return Created(nameof(SendMessageCommand), result);
        }
    }
}
