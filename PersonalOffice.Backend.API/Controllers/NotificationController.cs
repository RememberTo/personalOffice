using Microsoft.AspNetCore.Mvc;
using PersonalOffice.Backend.API.Models.Notify;
using PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetFilesById;
using PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotify;
using PersonalOffice.Backend.Application.CQRS.Notification.Queries.GetNotifyById;
using PersonalOffice.Backend.Domain.Entites.Pagination;

namespace PersonalOffice.Backend.API.Controllers
{
    /// <summary>
    /// Контроллер для рассылки уведомлений
    /// </summary>
    public class NotificationController : BaseController
    {
        /// <summary>
        /// Получение уведомлений
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/notify?count=12
        /// 
        /// </remarks>
        /// <param name="count">Количество уведомлений</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(NotificationsVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("notifies")]
        public async Task<ActionResult<NotificationsVm>> GetNotify([FromQuery] int count, [FromQuery] int pageNumber, CancellationToken cancellationToken)
        {
            var notifications = await Mediator.Send(new GetNotifyQuery 
            { 
                UserId = base.UserId, 
                PageConfig = new PageConfiguration { PageNumber = pageNumber, PageSize = count } 
            }, cancellationToken);
            
            return Ok(notifications);
        }

        /// <summary>
        /// Обновление статуса IsRead для уведомления
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     PATCH api/notifies/50309?markNotify=true
        /// 
        /// </remarks>
        /// <param name="notifyId">Идентификатор уведомления</param>
        /// <param name="markNotify">Сделать уведомление прочитанным по умолчанию true</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(NotificationsVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPatch("notifies/{notifyId}")]
        public async Task<ActionResult<NotificationsVm>> GetNotifyById([FromRoute] int notifyId, [FromBody] UpdateMarkNotify markNotify, CancellationToken cancellationToken)
        {
            var notification = await Mediator.Send(new NotifyMarkAsReadCommand
            {
                UserId = base.UserId,
                NotifyId = notifyId,
                MarkNotify = markNotify.IsRead,
            }, cancellationToken);

            return Ok(notification);
        }

        /// <summary>
        /// Получение уведомлений
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// 
        ///     GET api/notifies/files/50309
        /// 
        /// </remarks>
        /// <param name="notifyId">Идентификатор уведомления</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен</response>
        /// <response code="400">Ошибка API</response>
        [ProducesResponseType(typeof(NotificationsVm), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpGet("notifies/{notifyId}/files")]
        public async Task<ActionResult<NotificationsVm>> GetNotifyFiles([FromRoute] int notifyId, CancellationToken cancellationToken)
        {
            var notifications = await Mediator.Send(new GetFilesQuery 
            { 
                UserId = base.UserId, 
                NotifyId = notifyId
            }, cancellationToken);

            return Ok(notifications);
        }
    }
}
