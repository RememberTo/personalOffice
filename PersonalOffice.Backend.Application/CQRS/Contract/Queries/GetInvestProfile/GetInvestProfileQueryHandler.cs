using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetInvestProfile
{
    internal class GetInvestProfileQueryHandler(
        ILogger<GetInvestProfileQueryHandler> logger,
        IContractService contractService,
        ITransportService transportService) : IRequestHandler<GetInvestProfileQuery, InvestProfileVm>
    {
        private readonly ILogger<GetInvestProfileQueryHandler> _logger = logger;
        private readonly IContractService _contractService = contractService;
        private readonly ITransportService _transportService = transportService;

        public async Task<InvestProfileVm> Handle(GetInvestProfileQuery request, CancellationToken cancellationToken)
        {
            await _contractService.CheckContract(request.ContractId, request.UserId, cancellationToken);

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<IEnumerable<InvestProfileVm>>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "PO_GetInvestProfile",
                Data = request.ContractId,
            }, cancellationToken)).Data);

            if (sqlResult == null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка получения данных для инвестпрофиля: {msg}", sqlResult?.Message);
                throw new InvalidOperationException($"Не удалось получить информацию об инвестпрофиле");
            }
            if (sqlResult.ReturnValue is null)
                throw new NotFoundException("Инвестпрофиль не найден");

            return sqlResult.ReturnValue.First();
        }
    }
}
