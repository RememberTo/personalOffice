
namespace PersonalOffice.Backend.Application.CQRS.User.Commands
{
    /// <summary>
    /// ������ ������������� ������ �������
    /// </summary>
    public class AccessTokenVm
    {
        /// <summary>
        /// JWT
        /// </summary>
        public string? Token { get; set; }
    }
}