using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Application.CQRS.Report.General;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Report.Commands.CreateCustomReport
{
    internal class CreateCustomReportCommandHandler(
        ILogger<CreateCustomReportCommandHandler> logger,
        ITransportService transportService,
        IUserService userService) : IRequestHandler<CreateCustomReportCommand, CustomReportVm>
    {
        private readonly ILogger<CreateCustomReportCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IUserService _userService = userService;

        public async Task<CustomReportVm> Handle(CreateCustomReportCommand request, CancellationToken cancellationToken)
        {
            request.ReportID = Guid.NewGuid().ToString();
            _logger.LogTrace("Начало создания отчета параметры ReportID:{rid} ContractID:{cid} UserId:{uid} ReportType:{rt}",
                request.ReportID, request.ContractID, request.UserId, request.ReportType);
            
            var docNum = await _userService.GetDocNumAsync(request.ContractID, request.UserId);

            await _transportService.SendMessageAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.ReportMaster,
                Method = "CustomReport",
                Data = request
            });

            _logger.LogTrace("Отчет ReportID:{rid} на этапе создания", request.ReportID);

            var date = request.BeginDate == request.EndDate ? request.BeginDate.ToString("dd.MM.yyyy")
                : request.BeginDate.ToString("dd.MM.yyyy") + "-" + request.EndDate.ToString("dd.MM.yyyy");

            return new CustomReportVm
            {
                ReportId = request.ReportID,
                Name = docNum + " " + request.ReportTypeName + " за " + date
            };
        }
    }
}
