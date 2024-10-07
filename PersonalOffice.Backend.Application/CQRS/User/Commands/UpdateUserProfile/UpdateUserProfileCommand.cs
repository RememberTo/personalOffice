using MediatR;
using PersonalOffice.Backend.Domain.Entites.User;

namespace PersonalOffice.Backend.Application.CQRS.User.Commands.UpdateProfile
{
    /// <summary>
    /// Контракт на обновление профиля пользователя
    /// </summary>
    public class UpdateUserProfileCommand : IRequest<UserProfile>
    {
        /// <summary>
        /// Идентфикатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Обновленная модель пользователя
        /// </summary>
        public required UserProfile UpdateProfile { get; set; }
    }
}
