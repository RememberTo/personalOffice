namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserBranches
{
    /// <summary>
    /// Модель представления веток пользователя
    /// </summary>
    public class UserBranchesVm
    {
        /// <summary>
        /// Присутсвуют ли привелегии ИИС
        /// </summary>
        public bool IsPrivilegiesIIS { get; set; }
        /// <summary>
        /// ветки пользователя
        /// </summary>
        public required IEnumerable<BranchVm> Branches { get; set; }
    }
}
