using MediatR;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetVariantPayment
{
    /// <summary>
    /// Контракт на получение списка вариантов оплаты
    /// </summary>
    public class GetPaymentOptionsQuery : IRequest<IEnumerable<PaymentOptionsVm>>
    {
        /// <summary>
        /// Идентификатор пользвоателя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int ContractId { get; set; }
    }
}
