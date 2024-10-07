using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Security.Queries.GetSignType
{
    internal class GetSignTypeQueryHandler(
        ILogger<GetSignTypeQueryHandler> logger,
        ITransportService transportService,
        IUserService userService) : IRequestHandler<GetSignTypeQuery, SignTypeVm>
    {
        private readonly ILogger<GetSignTypeQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IUserService _userService = userService;

        public async Task<SignTypeVm> Handle(GetSignTypeQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Запрос информации о пользоватле");
            var user = await _userService.GetGeneralUserInfoAsync(request.UserId, cancellationToken);

            _logger.LogTrace("Запрос информации о типе подписи");
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "DOC_IsAnyContractAdditionalSignSMS",
                Data = request.UserId
            }, cancellationToken);

            var isSmsSign = (bool)msg.Data;

            _logger.LogTrace("Пользователь с типом подписи {}", isSmsSign ? "СМС" : "Сертификат");

            return new SignTypeVm
            {
                SignType = isSmsSign || user.IsPhysic ? SignType.SmsCode : SignType.Eds
            };
        }
    }
}
