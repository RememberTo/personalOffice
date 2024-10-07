using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.OneTimePass.Commands.SendOtp;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTests;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за работу с одноразовыми кодами
    /// </summary>
    public class OneTimePassController : BaseController
    {
        /// <summary>
        /// Отправка одноразового кода
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/otp/send
        ///     
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Список тестов</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost("otp/send")]
        public async Task<IActionResult> SendOtp(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new SendOtpCommand { UserId = base.UserId }, cancellationToken);

            return Ok(result);
        }
    }
}
