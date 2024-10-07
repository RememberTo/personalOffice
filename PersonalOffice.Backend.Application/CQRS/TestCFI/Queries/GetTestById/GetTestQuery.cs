using MediatR;
using PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTestById.Vm;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetTestById
{
    /// <summary>
    /// Контракт на получени теста
    /// </summary>
    public class GetTestQuery : IRequest<TestVm>
    {
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public int TestId { get; set; }
    }
}
