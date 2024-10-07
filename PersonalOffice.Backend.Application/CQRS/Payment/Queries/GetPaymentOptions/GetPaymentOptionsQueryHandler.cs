using MediatR;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetVariantPayment
{
    internal class GetPaymentOptionsQueryHandler(IContractService contractService) : IRequestHandler<GetPaymentOptionsQuery, IEnumerable<PaymentOptionsVm>>
    {
        private readonly IContractService _contractService = contractService;

        public async Task<IEnumerable<PaymentOptionsVm>> Handle(GetPaymentOptionsQuery request, CancellationToken cancellationToken)
        {
            var contract = await _contractService.GetContractById(request.UserId, request.ContractId, cancellationToken);

            var payOptions = new List<PaymentOptionsVm>
            {
                new() { Id = 1, Name = "По QR-коду" },
                new() { Id = 2, Name = "По реквизитам" },
            };

            if (contract.IsDP)
                payOptions.Add(new() { Id = 0, Name = "С использованием СБП" });

            return payOptions.OrderBy(x => x.Id);
        }
    }
}
