using MediatR;
using Newtonsoft.Json;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.User.Commands.ChangePassword
{
    /// <summary>
    /// Контракт на смену пароля
    /// </summary>
    public class ChangePasswordCommand : IRequest<IResult>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.PO_SetPasswordRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("UserID")]
        public required int UserId { get; set; }
        /// <summary>
        /// Хэш старого пароля
        /// </summary>
        [JsonIgnore]
        public required string OldPasswordHash { get; set; }
        /// <summary>
        /// Хэш нового пароля
        /// </summary>
        [JsonProperty("Password")]
        public required string NewPasswordHash { get; set; }
    }
}
