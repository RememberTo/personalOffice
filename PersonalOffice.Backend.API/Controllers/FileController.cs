using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonalOffice.Backend.Application.CQRS.File.Queries;
using PersonalOffice.Backend.Application.CQRS.File.Queries.GetCustomReportFile;
using PersonalOffice.Backend.Application.CQRS.File.Queries.GetDocFile;
using PersonalOffice.Backend.Application.CQRS.File.Queries.GetReportFile;
using PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с файлами, загрузка и отправка
    /// </summary>
    public class FileController :  BaseController, IActionFilter
    {
        /// <summary>
        /// Выполнение до вызова метода контроллера
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        [ApiExplorerSettings(IgnoreApi = true)]
        public void OnActionExecuting(ActionExecutingContext context) { }

        /// <summary>
        /// Выполнение после вызова метода контроллера
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        [ApiExplorerSettings(IgnoreApi = true)]
        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "content-disposition");
        }

        /// <summary>
        /// Получение файла из уведомления
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/files/notifies/1111
        /// 
        /// </remarks>
        /// <param name="id">Идентификатор файла из уведомления</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("files/notifies/{id}")]
        public async Task<IActionResult> GetNotifyFileById(int id)
        {
            var file = await Mediator.Send(new GetNotifyFileByIdQuery { FileId = id, UserId = base.UserId });

            return File(file.Content, file.ContentType, file.FileName);
        }

        /// <summary>
        /// Получение файла отчета
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/files/reports/1111
        /// 
        /// </remarks>
        /// <param name="id">Идентификатор отчета</param>
        /// <param name="isSig">Параметр который задает необъодимый файл, если false то вернется отчет, если true то вернется файл с ключем подписи</param>
        /// <param name="fileName">Желаемое название файла</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("files/reports/{id}")]
        public async Task<IActionResult> GetReportFileById([FromRoute] int id, [FromQuery] bool isSig, [FromQuery] string? fileName)
        {
            var file = await Mediator.Send(new GetReportFileQuery { FileId = id, UserId = base.UserId, FileName = fileName, IsSignFile = isSig });

            return File(file.Content, file.ContentType, file.FileName);
        }

        /// <summary>
        /// Получение файла, созданного отчета
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/files/reports/custom/guid-1g-uid
        /// 
        /// </remarks>
        /// <param name="id">Идентификатор созданного отчета</param>
        /// <param name="fileName">Желаемое название файла</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("files/reports/custom/{id}")]
        public async Task<IActionResult> GetCustomReportFileById([FromRoute] string id, [FromQuery] string? fileName)
        {
            var file = await Mediator.Send(new GetCustomReportFileQuery { UserId = base.UserId, ReportId = id, FileName = fileName });

            return File(file.Content, file.ContentType, file.FileName);
        }

        /// <summary>
        /// Получение файла из документа
        /// </summary>
        /// <param name="id">Идентфиикатор файла</param>
        /// <param name="fileName">Желаемое название файла</param>
        /// <returns></returns>
        [HttpGet("files/docs/{id}")]
        public async Task<IActionResult> GetDocumentFileById([FromRoute] int id, [FromQuery] string? fileName)
        {
            var file = await Mediator.Send(new GetDocFileQuery { FileId = id, UserId = base.UserId, FileName = fileName });

            return File(file.Content, file.ContentType, file.FileName);
        }
    }
}
