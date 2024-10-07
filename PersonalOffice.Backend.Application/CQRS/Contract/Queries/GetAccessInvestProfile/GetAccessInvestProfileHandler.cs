using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.SQL;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Contract.Queries.GetAccessInvestProfile
{
    internal class GetAccessInvestProfileHandler(
        ILogger<GetAccessInvestProfileHandler> logger,
        ITransportService transportService) : IRequestHandler<GetAccessInvestProfile, IResult>
    {
        private readonly ILogger<GetAccessInvestProfileHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<IResult> Handle(GetAccessInvestProfile request, CancellationToken cancellationToken)
        {
            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<bool>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_InvestAllow",
                Data = new IntRequest { Value = request.ContractId },
            }, cancellationToken)).Data);

            if (sqlResult == null || !sqlResult.Success)
            {
                _logger.LogError("Ошибка при определении доступности подачи инвест-анкеты: {msg}", sqlResult?.Message);
                throw new InvalidOperationException($"Не удалось получить информацию о доступе к подаче инвест-анкеты");
            }

            return new Result(sqlResult.ReturnValue ? InternalStatus.AccessAllowed : InternalStatus.AccessDenied);
        }
    }
}
