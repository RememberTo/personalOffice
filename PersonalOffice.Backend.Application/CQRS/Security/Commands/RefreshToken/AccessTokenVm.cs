
namespace PersonalOffice.Backend.Application.CQRS.User.Commands
{
    /// <summary>
    /// ћодель представлени€ токена доступа
    /// </summary>
    public class AccessTokenVm
    {
        /// <summary>
        /// JWT
        /// </summary>
        public string? Token { get; set; }
    }
}