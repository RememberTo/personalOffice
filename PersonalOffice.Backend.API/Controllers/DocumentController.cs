using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Document;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateArbitraryDocument;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateInvestProfileDocument;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.DeleteDocument;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.SetStatusDocument;
using PersonalOffice.Backend.Application.CQRS.Document.Commands.SignDocument;
using PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById;
using PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocById.Vm;
using PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocHash;
using PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocs;
using PersonalOffice.Backend.Application.CQRS.File.Commands;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с документами
    /// </summary>
    public class DocumentController(
        ILogger<DocumentController> logger,
        IMapper mapper) : BaseController
    {
        private readonly ILogger<DocumentController> _logger = logger;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Получение списка зарегестрированных документов
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/docs
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список документов</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(DocumentsInfoVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("docs")]
        public async Task<ActionResult<DocumentsInfoVm>> GetDocs([FromQuery] int pageNum, [FromQuery] int count, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetDocsQuery { UserId = base.UserId, Page = pageNum, PageSize = count }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Получение документа
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/docs/8573
        ///     TypeId 1 - userid=38310 docid=39
        ///     TypeId 2 - userid=31729 docid=1127
        ///     TypeId 3 - userid=2339  docid=8573
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Документ</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(DocumentBaseVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("docs/{docId}")]
        public async Task<ActionResult<DocumentBaseVm>> GetDoc(int docId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetDocByIdQuery { UserId = base.UserId, DocId = docId }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Изменение статуса документа
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PATCH api/docs/8573
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Результат смены статуса</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Domain.Interfaces.Objects.IResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPatch("docs/{docId}")]
        public async Task<ActionResult<Domain.Interfaces.Objects.IResult>> SetStatusDoc(int docId, [FromBody] SetStatusModel model, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SetStatusDocumentCommand { UserId = base.UserId, DocumentId = docId, Status = model.Status, StatusComment = model.CommentStatus }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Удаление документа
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     DELETE api/docs/8573
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Результат удаления</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Domain.Interfaces.Objects.IResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpDelete("docs/{docId}")]
        public async Task<ActionResult<Domain.Interfaces.Objects.IResult>> DeleteDoc(int docId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteDocumentCommand { UserId = base.UserId, DocumentId = docId }, cancellationToken);

            return Ok(result);
        }


        /// <summary>
        /// Подписание документа
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/docs/8573/sign
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Результат подписания</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Domain.Interfaces.Objects.IResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("docs/{docId}/sign")]
        public async Task<ActionResult<Domain.Interfaces.Objects.IResult>> SignDoc(int docId, SignDocumentDto signDocumentDto, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<SignDocumentCommand>(signDocumentDto);
            command.UserId = base.UserId;
            command.DocumentId = docId;

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Получение хэша документа
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/docs/8573/hash
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Хэш документа</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(DocHashVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("docs/{docId}/hash")]
        public async Task<ActionResult<DocHashVm>> GetHashDoc(int docId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetDocumentHashQuery { DocumentId = docId, UserId = base.UserId }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Создание произвольного(иного) документа
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/docs/arbitrary
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Результат и идентификатор документа</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Domain.Interfaces.Objects.IResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("docs/arbitrary")]
        [RequestSizeLimit(500_000_000)] // 500мб
        public async Task<ActionResult<Domain.Interfaces.Objects.IResult>> CreateArbitraryDoc([FromForm] CreateArbitraryDocModel model, List<IFormFile> files, CancellationToken cancellationToken)
        {
            _logger.LogTrace("ContractId {cid}, Name {nm}, Comment {cm}, CountFiles {cntf}", model.ContractId, model.Name, model.Comment, files.Count);
           
            var command = _mapper.Map<CreateArbitraryDocumentCommand>(model);
            command.UploadFiles = _mapper.Map<ICollection<UploadFile>>(files);
            command.UserId = base.UserId;

            var result = await Mediator.Send(command, cancellationToken);

            return Created("", result);
        }

        /// <summary>
        /// Создание анкеты инвестиционного профиля
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/docs/invest-profile
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Результат и идентификатор документа</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Domain.Interfaces.Objects.IResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("docs/invest-profile")]
        [RequestSizeLimit(500_000_000)] // 500мб
        public async Task<ActionResult<Domain.Interfaces.Objects.IResult>> CreateInvestProfileDoc([FromBody] CreateInvestProfileDocModel model, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateInvestProfileDocumentCommand>(model);
            command.UserId = base.UserId;

            var result = await Mediator.Send(command, cancellationToken);

            return Created("", result);
        }


    }
}
