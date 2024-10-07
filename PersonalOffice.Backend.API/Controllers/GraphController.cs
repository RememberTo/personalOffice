using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Graph;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.General;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractAllGraphs;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractGraph;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetGeneralGraph;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с графиками
    /// </summary>
    public class GraphController(
        IMapper mapper) : BaseController
    {
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Данные для построения графика по определенному договору
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/graph/contract/906
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список данных для построения графика</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<PointGraphVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("graph/contract/{contractId}")]
        public async Task<ActionResult<IEnumerable<PointGraphVm>>> GetContractGraphData(int contractId, [FromQuery] AnalyticsQueryModel model, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetContractGraphQuery>(model);
            request.ContractId = contractId;
            request.UserId = base.UserId;

            var result = await Mediator.Send(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Данные для построения всех графиков по договору
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/graph/contract/906/all
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список данных для построения графика</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<AllGraphVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("graph/contract/{contractId}/all")]
        public async Task<ActionResult<IEnumerable<AllGraphVm>>> GetAllContractGraphsData(int contractId, [FromQuery] AnalyticsQueryModel model, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetContractAllGraphsQuery>(model);
            request.UserId = base.UserId;
            request.ContractId = contractId;

            var result = await Mediator.Send(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Данные для построения общего графика
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/graph/general
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список данных для построения графика</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<PointGraphVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("graph/general")]
        public async Task<ActionResult<IEnumerable<PointGraphVm>>> GetGeneralGraphData([FromQuery] AnalyticsQueryModel model, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetGeneralGraphQuery>(model);
            request.UserId = base.UserId;

            var result = await Mediator.Send(request, cancellationToken);

            return Ok(result);
        }
    }
}
