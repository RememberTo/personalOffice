using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Question.Commands.SendMessage
{
    internal class SendMessageCommandHandler(
        ILogger<SendMessageCommandHandler> logger,
        ITransportService transportService,
        IUserService userService)
        : IRequestHandler<SendMessageCommand, IResult>
    {
        private readonly ILogger<SendMessageCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IUserService _userService = userService;

        public async Task<IResult> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            await _userService.CheckUserTopicAccessAsync(request.UserId, request.TopicId, cancellationToken);

            await _transportService.SendMessageAsync(new Message
                    {
                        Source = MicroserviceNames.Backend,
                        Destination = MicroserviceNames.ManagerQuestion,
                        Method = "UserSendMessage",
                        Data = request
                    });


            return new Result(InternalStatus.Sent, "Сообщение отправлено");
        }
    }
}
