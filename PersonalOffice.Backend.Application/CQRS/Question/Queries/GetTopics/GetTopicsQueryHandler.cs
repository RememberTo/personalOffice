using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.Question;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Question.Queries.GetTopics
{
    internal class GetTopicsQueryHandler(
        ILogger<GetTopicsQueryHandler> logger,
        ITransportService transportService)
        : IRequestHandler<GetTopicsQuery, IEnumerable<TopicVm>>
    {
        private readonly ILogger<GetTopicsQueryHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<IEnumerable<TopicVm>> Handle(GetTopicsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Начало получения сообщений из микросервиса: {mName} метод: {mMethod}",
               MicroserviceNames.ManagerQuestion, "AllTopics4User");

            var msg = await _transportService.RPCServiceAsync(
                new Message
                {
                    Source = MicroserviceNames.Backend,
                    Destination = MicroserviceNames.ManagerQuestion,
                    Method = "AllTopics4User",
                    Data = request
                }, cancellationToken);

            _logger.LogTrace("Результат получен микросервис: {mName} метод: {mMethod}",
               MicroserviceNames.ManagerQuestion, "AllTopics4User");

            var qmResult = Common.Global.Convert.DataTo<QuestionResult<IEnumerable<TopicVm>>>(msg.Data);

            if (!qmResult.Success)
                throw new InvalidOperationException(qmResult.Comment);

            return qmResult.Data ?? Enumerable.Empty<TopicVm>();
        }
    }
}
