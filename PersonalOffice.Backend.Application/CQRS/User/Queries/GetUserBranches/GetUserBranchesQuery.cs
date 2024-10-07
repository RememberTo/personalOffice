using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserBranches
{
    /// <summary>
    /// Контракт на получение веток пользователя
    /// </summary>
    public class GetUserBranchesQuery : IRequest<UserBranchesVm> 
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
    }
}
