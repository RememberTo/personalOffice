using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetProfile
{
    internal class GetUserProfileQueryHandler(
        ILogger<GetUserProfileQueryHandler> logger,
        IMapper mapper,
        IUserService userService) : IRequestHandler<GetUserProfileQuery, UserProfileVm>
    {
        private readonly ILogger<GetUserProfileQueryHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IUserService _userService = userService;

        public async Task<UserProfileVm> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _userService.GetUserProfileAsync(request.UserId, cancellationToken);

            return _mapper.Map<UserProfileVm>(userProfile);
        }
    }
}
