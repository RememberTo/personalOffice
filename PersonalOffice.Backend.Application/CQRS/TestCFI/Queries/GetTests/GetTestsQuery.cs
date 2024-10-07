using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTests
{
    /// <summary>
    /// Контракт на получение списка информации о тестах
    /// </summary>
    public class GetTestsQuery : IRequest<IEnumerable<TestVm>>
    {
        /// <summary>
        /// Индентификатор пользователя нужен для определения пройденных не проейденных тестов
        /// </summary>
        public required int UserID { get; set; }
    }
}
