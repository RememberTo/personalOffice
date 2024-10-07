using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetProfile
{
    /// <summary>
    /// Контракт на запрос информации о профиле пользователя
    /// </summary>
    public class GetUserProfileQuery : IRequest<UserProfileVm>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
    }
}
