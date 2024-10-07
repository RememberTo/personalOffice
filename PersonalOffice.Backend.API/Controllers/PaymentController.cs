using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Graph;
using PersonalOffice.Backend.API.Models.Payment;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.General;
using PersonalOffice.Backend.Application.CQRS.Graph.Queries.GetGeneralGraph;
using PersonalOffice.Backend.Application.CQRS.Payment.Commands.TopUpAccount;
using PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetPayments;
using PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetVariantPayment;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    public class PaymentController(
        IMapper mapper) : BaseController
    {
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Список истории пополнения счета
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/payments
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список пополнения счета</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<PaymentVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("payments")]
        public async Task<ActionResult<IEnumerable<PaymentVm>>> GetPayments(int page, int pageSize, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetPaymentsQuery { UserId = base.UserId, Page = page, PageSize = pageSize }, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Пополнение счета
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     POST api/payments
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Файл в формате base64, для пополнения счета</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(TopUpVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("payments")]
        public async Task<ActionResult<TopUpVm>> CreateQrCode([FromBody] TopUpModel model, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<TopUpAccountCommand>(model);
            command.UsertId = base.UserId;

            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Список вариантов пополнения счета
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/payments/options
        /// 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список вариантов для пополнения</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(IEnumerable<PaymentOptionsVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("payments/options")]
        public async Task<ActionResult<IEnumerable<PaymentOptionsVm>>> GetPaymentOptions([FromQuery] int contractId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetPaymentOptionsQuery { ContractId = contractId, UserId = base.UserId }, cancellationToken);

            return Ok(result);
        }
    }
}
