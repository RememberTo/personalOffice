using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.User.Commands
{
    /// <summary>
    /// �������� �� ���������� Access ������
    /// </summary>
    public class RefreshAccessTokenCommand : IRequest<RefreshAccessTokenCommand>
    {
        /// <summary>
        /// ����� �������
        /// </summary>
        public string? AccessToken { get; set; }
    }
}