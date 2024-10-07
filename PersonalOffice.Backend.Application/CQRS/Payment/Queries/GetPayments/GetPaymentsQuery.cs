using MediatR;
using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetPayments
{
    /// <summary>
    /// Контракт на получение списка пополнений счета
    /// </summary>
    public class GetPaymentsQuery : IRequest<PaymentsInfoVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.DOC_ClientDocListRequest, MessageDataTypes";
        /// <summary>
        /// Идентфиикатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Вместимость 1 страницы
        /// </summary>
        public int PageSize { get; set; }
    }
}
