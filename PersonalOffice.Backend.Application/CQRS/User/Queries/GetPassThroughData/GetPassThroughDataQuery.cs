using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetPassThroughData
{
    /// <summary>
    /// Контракт на получение данных для бесшовного перехода
    /// </summary>
    public class GetPassThroughDataQuery : IRequest<PassThroughDto>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
    }
}
