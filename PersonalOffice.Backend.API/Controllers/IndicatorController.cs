using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Graph;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.General;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractAllGraphs;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetContractGraph;
using PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetContractIndicators;
using PersonalOffice.Backend.Application.CQRS.Indicator.Queries.GetGeneralIndicators;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с индикаторами
    /// </summary>
    /// <param name="mapper"></param>
    public class IndicatorController(
        IMapper mapper) : BaseController
    {
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Индикаторы договора
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/indicators/contract/906
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Индикаторы</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IndicatorsVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("indicators/contract/{contractId}")]
        public async Task<ActionResult<IndicatorsVm>> GetContractIndicators(int contractId, [FromQuery] AnalyticsQueryModel model, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetContractIndicatorsQuery>(model);
            request.ContractId = contractId;
            request.UserId = base.UserId;

            var result = await Mediator.Send(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Общие индикаторы
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/indicators/general
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Индикаторы</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<PointGraphVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("indicators/general")]
        public async Task<ActionResult<IndicatorsVm>> GetGeneralIndicators([FromQuery] AnalyticsQueryModel model, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetGeneralIndicatorsQuery>(model);
            request.UserId = base.UserId;

            var result = await Mediator.Send(request, cancellationToken);

            return Ok(result);
        }
    }
}
