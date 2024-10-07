using MediatR;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.File;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.File;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries.GetCustomReportFile
{
    internal class GetCustomReportFileQueryHandler(
        ITransportService transportService) : IRequestHandler<GetCustomReportFileQuery, FileVm>
    {
        private readonly ITransportService _transportService = transportService;

        public async Task<FileVm> Handle(GetCustomReportFileQuery request, CancellationToken cancellationToken)
        {
            var file = Common.Global.Convert.DataTo<FileOperationResult>((await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.FileManager,
                Method = "GetFileInMinIO",
                Data = new FileReadMinIORequest
                {
                    Bucket = "custom-report",
                    UserId = request.UserId,
                    FileName = request.ReportId
                }
            }, TimeSpan.FromMinutes(2))).Data);

            if (!file.Success)
            {
                if (file.Comment is null) throw new InvalidOperationException("Неизвестная операция");
                if (file.Comment.Contains("NotFound")) throw new NotFoundException("Файл не найден");
                else throw new InvalidOperationException(file.Comment);
            }

            var metadata = Common.Global.Convert.DataTo<Dictionary<string, string>>(file.Data ?? throw new InvalidOperationException("Информация о файле отсутсвует"));

            return new FileVm
            {
                Content = file.Content,
                FileName = string.IsNullOrEmpty(request.FileName) ? metadata["filename"] : request.FileName,
                ContentType = "multipart/form-data"
            };
        }
    }
}
