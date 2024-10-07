using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Question.Commands.SendMessage;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Question.Commands.CreateTopic
{
    internal class CreateTopicCommandHandler(
        ILogger<SendMessageCommandHandler> logger,
        ITransportService transportService)
        : IRequestHandler<CreateTopicCommand, IResult>
    {
        private readonly ILogger<SendMessageCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;

        public async Task<IResult> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            await _transportService.SendMessageAsync(new Message
                    {
                        Source = MicroserviceNames.Backend,
                        Destination = MicroserviceNames.ManagerQuestion,
                        Method = "AddTopic",
                        Data = request
                    });

            return new Result(InternalStatus.Created, "Топик создан");
        }
    }
}
