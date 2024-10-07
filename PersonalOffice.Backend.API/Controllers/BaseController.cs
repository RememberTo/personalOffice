using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Базовый класс для всех контроллеров, содержащий базовый роут, 
    /// медиатор, и индентификатор пользователя
    /// </summary>
    [ApiController]
    [Route("api/")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator? _mediator;
        /// <summary>
        /// Реализация паттерна проектирования
        /// Представляет интерфейс для взаимодействия с различными объектами для запросов и команд (CQRS)
        /// </summary>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator?>() ?? throw new InvalidOperationException();

        internal int UserId
        {
            get
            {
                if (User.Identity is null)
                    throw new ArgumentNullException(nameof(User.Identity));
           
                return User.Identity.IsAuthenticated ? Convert.ToInt32(User.FindFirst("UserId")?.Value) : throw new ArgumentNullException("UserId");
            }
        }
    }
}
