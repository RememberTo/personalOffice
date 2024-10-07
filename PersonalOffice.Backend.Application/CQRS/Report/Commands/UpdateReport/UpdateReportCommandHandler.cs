using MediatR;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Report.Commands.UpdateReport
{
    internal class UpdateReportCommandHandler(
        ITransportService transportService) : IRequestHandler<UpdateReportCommand, IResult>
    {
        private readonly ITransportService _transportService = transportService;

        public async Task<IResult> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsResetStatus) { return new Result(Domain.Common.Enums.InternalStatus.Success, request.FileId.ToString()); }

            var sqlResult = Common.Global.Convert.DataTo<SQLOperationResult<string?>>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "RP_ReportNewReset",
                Data = request.FileId
            }, cancellationToken)).Data);

            if (sqlResult == null || !sqlResult.Success) throw new InvalidOperationException($"Не удалось сбросить статус \"новый\" с файла {request.FileId}");

            return new Result(Domain.Common.Enums.InternalStatus.Success, request.FileId.ToString());
        }
    }
}
