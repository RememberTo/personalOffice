using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Queries.GetPayments
{
    internal class GetPaymentsQueryHandelr(
        ILogger<GetPaymentsQueryHandelr> logger,
        ITransportService transportService) : IRequestHandler<GetPaymentsQuery, PaymentsInfoVm>
    {
        private readonly ILogger<GetPaymentsQueryHandelr> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<PaymentsInfoVm> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "NSPK_TopUpAccountList",
                Data = request
            }, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<PaymentsInfoVm>>(msg.Data);

            if(!sqlResult.Success || sqlResult.ReturnValue is null)
            {
                _logger.LogError("Не удалось преобразовать данные: {msg}", sqlResult.Message);
                return new PaymentsInfoVm();
            }

            return sqlResult.ReturnValue;
        }
    }
}
