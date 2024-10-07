using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetPassThroughData
{
    internal class GetPassThroughDataQueryHandler(
        ILogger<GetPassThroughDataQueryHandler> logger,
        IClsCryptoService clsCryptoService,
        IUserService userService) : IRequestHandler<GetPassThroughDataQuery, PassThroughDto>
    {
        private readonly ILogger<GetPassThroughDataQueryHandler> _logger = logger;
        private readonly IClsCryptoService _clsCryptoService = clsCryptoService;
        private readonly IUserService _userService = userService;

        public async Task<PassThroughDto> Handle(GetPassThroughDataQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Получение логина пользователя");
            var login = await _userService.GetUserLoginAsync(request.UserId);

            _logger.LogTrace("Кодирование логина");
            var encryptString = _clsCryptoService.Encrypt(login);

            return new PassThroughDto 
            { 
                Encoded = encryptString, 
                СlientId = request.UserId 
            };
        }
    }
}
