using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.User.Commands
{
    internal class RefreshAccessTokenCommandHandler(
        ILogger<RefreshAccessTokenCommandHandler> logger, ITransportService transportService)
        : IRequestHandler<RefreshAccessTokenCommand, RefreshAccessTokenCommand>
    {
        private readonly ILogger<RefreshAccessTokenCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        public async Task<RefreshAccessTokenCommand> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var msg = await _transportService.RPCServiceAsync(
                        new Message
                        {
                            Source = MicroserviceNames.Backend,
                            Destination = MicroserviceNames.Authenticator,
                            Method = "NewAccessJWT",
                            Data = request.AccessToken ?? ""
                        }, TimeSpan.FromSeconds(5));

                return new RefreshAccessTokenCommand { AccessToken = msg.Data.ToString() };
            }
            catch (Exception ex)
            {
                _logger.LogError("Сообщение {exMsg}", ex.Message);
                throw;
            }
        }
    }
}