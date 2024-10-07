using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetAccessInvestProfile;
using PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContract;
using PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetContractList;
using PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetInvestProfile;
using PersonalOffice.Backend.Application.CQRS.Document.Queries.GetDocs;
using PersonalOffice.Backend.Domain.Enums;
using IResult = PersonalOffice.Backend.Domain.Interfaces.Objects.IResult;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с договорами
    /// </summary>
    public class ContractController : BaseController
    {
        /// <summary>
        /// Получение данных об инвестпрофиле
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/contracts/906
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Инвестпрофиль</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(InvestProfileVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("contracts/{contractId}/invest-profile")]
        public async Task<ActionResult<InvestProfileVm>> GetDocs(int contractId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetInvestProfileQuery { UserId = base.UserId, ContractId = contractId }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Разрешен ли доступ к инвест-анкете
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/contracts/906/invest-allow
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Доступ к инвест-анкете</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("contracts/{contractId}/invest-allow")]
        public async Task<ActionResult<IResult>> GetInvestProfileAllow(int contractId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetAccessInvestProfile { UserId = base.UserId, ContractId = contractId }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Получение информации о договоре
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/contracts/906
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Договор</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<ContractVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("contracts/{contractId}")]
        public async Task<ActionResult<IEnumerable<ContractVm>>> GetContractInfo(int contractId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetContractQuery { UserId = base.UserId, ContractId = contractId }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Получение информации о договоре
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/contracts
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Договор</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(ContractVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("contracts")]
        public async Task<ActionResult<ContractVm>> GetContracts([FromQuery] Currency? currency, [FromQuery] ContractType? type, CancellationToken cancellationToken)
        {
            var request = new GetContractListQuery { UserId = base.UserId };
            
            if (type is not null) 
                request.ContractType = type.Value;
            if (currency is not null)
                request.Currency = currency.Value;

            var result = await Mediator.Send(request, cancellationToken);

            return Ok(result);
        }
    }
}
