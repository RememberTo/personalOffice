using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.User.Commands
{
    /// <summary>
    /// Контракт на обновление Access токена
    /// </summary>
    public class RefreshAccessTokenCommand : IRequest<RefreshAccessTokenCommand>
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string? AccessToken { get; set; }
    }
}