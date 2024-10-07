using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Question.Queries.GetMessagesFromTopic;
using PersonalOffice.Backend.Domain.Entites.Question;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopicById
{
    internal class GetTopicQueryHandler(
        ILogger<GetMessagesQueryHandler> logger,
        ITransportService transportService,
        IUserService userService)
        : IRequestHandler<GetTopicByIdQuery, TopicInfoVm>
    {
        private readonly ILogger<GetMessagesQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IUserService _userService = userService;

        public async Task<TopicInfoVm> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            await _userService.CheckUserTopicAccessAsync(request.UserId, request.TopicId, cancellationToken);

            _logger.LogTrace("Начало получения сообщений из микросервиса: {mName} метод: {mMethod} Id топика {topicID}",
                MicroserviceNames.ManagerQuestion, "GetTopicInfo", request.TopicId);

            var msg = await _transportService.RPCServiceAsync(
                new Message
                {
                    Source = MicroserviceNames.Backend,
                    Destination = MicroserviceNames.ManagerQuestion,
                    Method = "GetTopicInfo",
                    Data = request.TopicId,
                }, cancellationToken);

            var qmResult = Common.Global.Convert.DataTo<QuestionResult<TopicInfoVm>>(msg.Data);

            if (!qmResult.Success)
                throw new InvalidOperationException(qmResult.Comment);

            return qmResult.Data ?? new TopicInfoVm();
        }
    }
}
