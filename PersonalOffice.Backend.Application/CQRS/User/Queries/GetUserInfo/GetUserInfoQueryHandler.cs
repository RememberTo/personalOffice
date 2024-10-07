using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserInfo
{
    internal class GetUserInfoQueryHandler(
        ILogger<GetUserInfoQueryHandler> logger,
        IUserService userService)
        : IRequestHandler<GetUserInfoQuery, UserInfoVm>
    {
        private readonly ILogger<GetUserInfoQueryHandler> _logger = logger;
        private readonly IUserService _userService = userService;

        public async Task<UserInfoVm> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения сообщений из микросервиса: {mName} метод: {mMethod}",
               MicroserviceNames.ManagerQuestion, "AllTopics4User");

            bool? canContactInvestmentConsultant = await _userService.CanContactInvestmentConsultantAsync(request.UserID, cancellationToken);

            decimal totalPortfolioValue = await _userService.GetTotalPortfolioValueAsync(request.UserID, cancellationToken);

            var res = new UserInfoVm() { 
                CanContactInvestmentConsultant = canContactInvestmentConsultant ?? false, 
                TotalPortfolioValue = totalPortfolioValue,
                UserId = request.UserID 
            };

            return res;
            //return null //qmResult.Data ?? Enumerable.Empty<TopicVm>();
        }
    }

}

