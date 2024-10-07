using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetCertificates
{
    internal class GetCertificatesQueryHandler(
        ILogger<GetCertificatesQueryHandler> logger,
        IMapper mapper,
        IUserService userService) : IRequestHandler<GetCertificatesQuery, IEnumerable<CertificateVm>>
    {
        private readonly ILogger<GetCertificatesQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<CertificateVm>> Handle(GetCertificatesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Получение общей информации о пользователе");
            var user = await _userService.GetGeneralUserInfoAsync(request.UserId, cancellationToken);

            _logger.LogTrace("Получение сертификатов пользователя");
            await _userService.AddCertificatesAsync(user, cancellationToken);

            return user.Certificates.Select(c => _mapper.Map<CertificateVm>(c));
        }
    }
}
