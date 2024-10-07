using MediatR;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.OneTimePass.Commands.SendOtp
{
    /// <summary>
    /// 
    /// </summary>
    public class SendOtpCommand : IRequest<IResult>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
    }
}
