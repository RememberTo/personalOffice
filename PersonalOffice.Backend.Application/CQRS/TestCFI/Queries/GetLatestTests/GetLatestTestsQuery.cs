using MediatR;
using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.TestCFI.Queries.GetLatestTests
{
    /// <summary>
    /// Контракт на получение недавно пройденных тестов
    /// </summary>
    public class GetLatestTestsQuery : IRequest<LatestTestsVm>
    {
        [JsonProperty("$type")]
        private string DeserizlizeType => "MessageDataTypes.OR_Orders4UserRequest, MessageDataTypes";
        /// <summary>
        /// Идентифиактор пользователя
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Статус теста
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Текущая страница
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Колисетво страниц
        /// </summary>
        public int PageSize { get; set; }
    }
}
