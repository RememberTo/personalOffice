

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Report;
using PersonalOffice.Backend.API.Models.Report.Custom;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Report.Commands.CreateCustomReport;
using PersonalOffice.Backend.Application.CQRS.Report.Commands.UpdateReport;
using PersonalOffice.Backend.Application.CQRS.Report.General;
using PersonalOffice.Backend.Application.CQRS.Report.Queries.GetCustomReport;
using PersonalOffice.Backend.Application.CQRS.Report.Queries.GetListCustomReport;
using PersonalOffice.Backend.Application.CQRS.Report.Queries.GetReports;
using PersonalOffice.Backend.Domain.Entites.Report;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за отчеты
    /// </summary>
    /// <param name="mapper"></param>
    public class ReportController(IMapper mapper) : BaseController
    {
        /// <summary>
        /// Получение списка отчетов
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/reports?count=10
        /// 
        /// </remarks>
        /// <param name="count">Количество элементов на странице</param>
        /// <param name="pageNum">Номер страницы</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(ReportsVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("reports")]
        public async Task<IActionResult> GetReportList([FromQuery] int count, [FromQuery] int pageNum)
        {
            var result = await Mediator.Send(new GetReportsQuery { UserId = base.UserId, Count = count, PageNum = pageNum });

            return Ok(result);
        }

        /// <summary>
        /// Получение списка всех созданных отчетов 
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/reports/custom
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<CustomReportVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("reports/custom")]
        public async Task<IActionResult> GetListCustomReport()
        {
            var result = await Mediator.Send(new GetListCustomReportQuery { UserId = base.UserId });

            return Ok(result);
        }

        /// <summary>
        /// Сброс статуса "новый" с файла отчета
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PATCH api/reports/1111
        /// 
        /// </remarks>
        /// <param name="fileId">Идентификатор файла</param>
        /// <param name="statusFile">Модель статуса файла</param>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Domain.Interfaces.Objects.IResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPatch("reports/{fileId}")]
        public async Task<IActionResult> GetReportList([FromRoute] int fileId, [FromBody] StatusFile statusFile)
        {
            var result = await Mediator.Send(new UpdateReportCommand { UserId = base.UserId, FileId = fileId, IsResetStatus = statusFile.IsResetStatus });

            return Ok(result);
        }

        /// <summary>
        /// Создание собственного отчета
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/reports/custom
        ///     {
        ///         "contractID": 0,
        ///         "beginDate": "2024-03-19T11:08:00.822Z",
        ///         "endDate": "2024-03-19T11:08:00.822Z",
        ///         "reportType": "1",
        ///         "reportTypeName": "Отчет",
        ///         "reportFormat": "pdf",
        ///         "language": "1",
        ///         "currency": "1",
        ///         "priceType": "1"
        ///     }
        /// 
        /// </remarks>
        /// <param name="customReportDto">Контракт на создание собственного отчета</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(CustomReport), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("reports/custom")]
        public async Task<IActionResult> CreateCustomReport([FromBody] CustomReportDto customReportDto)
        {
            var mappedDto = mapper.Map<CreateCustomReportCommand>(customReportDto);
            mappedDto.UserId = base.UserId;

            var result = await Mediator.Send(mappedDto);

            return Created("", result);
        }

        /// <summary>
        /// Получение информации о созданном отчете по id 
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/reports/guid-11gui-d/custom
        /// 
        /// </remarks>
        /// <param name="reportId">Идентификатор созданного отчета</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(CustomReportVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("reports/{reportId}/custom")]
        public async Task<IActionResult> GetCustomReport([FromRoute] string reportId)
        {
            var result = await Mediator.Send(new GetCustomReportQuery { UserId = base.UserId, ReportId = reportId });

            if (result is null) return new NotFoundResult();

            return Ok(result);
        }
    }
}
