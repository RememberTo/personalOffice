using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.Question;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetMessagesFromTopic
{
    internal class GetMessagesQueryHandler(
        ILogger<GetMessagesQueryHandler> logger,
        ITransportService transportService,
        IUserService userService)
        : IRequestHandler<GetMessagesQuery, IEnumerable<TopicMessageVm>>
    {
        private readonly ILogger<GetMessagesQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IUserService _userService = userService;

        public async Task<IEnumerable<TopicMessageVm>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            await _userService.CheckUserTopicAccessAsync(request.UserId, request.TopicId, cancellationToken);

            _logger.LogTrace("Начало получения сообщений из микросервиса: {mName} метод: {mMethod} Id топика {topicID}",
                MicroserviceNames.ManagerQuestion, "UserMessages4Topic", request.TopicId);

            var msg = await _transportService.RPCServiceAsync(
                new Message
                {
                    Source = MicroserviceNames.Backend,
                    Destination = MicroserviceNames.ManagerQuestion,
                    Method = "UserMessages4Topic",
                    Data = request.TopicId
                }, cancellationToken);

            _logger.LogTrace("Сообщение получено из микросервиса: {mName} метод: {mMethod} Id топика {topicID}",
               MicroserviceNames.ManagerQuestion, "UserMessages4Topic", request.TopicId);

            var qmResult = Common.Global.Convert.DataTo<QuestionResult<IEnumerable<TopicMessageVm>>>(msg.Data);

            if (!qmResult.Success)
                throw new InvalidOperationException(qmResult.Comment);

            return qmResult.Data ?? [];
        }
    }
}
