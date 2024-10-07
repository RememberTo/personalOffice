using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Common.Enums;
using PersonalOffice.Backend.Domain.Entities.Document;
using PersonalOffice.Backend.Domain.Entities.OneTimePass;
using PersonalOffice.Backend.Domain.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Objects;
using PersonalOffice.Backend.Domain.Interfaces.Services;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.SignDocument
{
    internal class SignDocumentCommandHandler(
        ILogger<SignDocumentCommandHandler> logger,
        IUserService userService,
        IDocumentService documentService,
        IOneTimePassService otpService) : IRequestHandler<SignDocumentCommand, IResult>
    {
        private readonly ILogger<SignDocumentCommandHandler> _logger = logger;
        private readonly IUserService _userService = userService;
        private readonly IDocumentService _documentService = documentService;
        private readonly IOneTimePassService _otpService = otpService;

        public async Task<IResult> Handle(SignDocumentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Получение информации о пользователе");
            var user = await _userService.GetGeneralUserInfoAsync(request.UserId, cancellationToken);

            _logger.LogWarning("Получение информации о документе с id: {did}", request.DocumentId);
            var doc = await _documentService.GetDocumentInfoAsync(new DocumentInfoRequest { DocId = request.DocumentId, UserId = user.UserId }, cancellationToken);
            string? signature;

            if (request.SignType == SignType.SmsCode && request.SmsCode is not null)
            {
                if (!user.IsPhoneConfirmed) throw new BadRequestException("Номер телефона не подтвержден");

                await _otpService.CheckOtpAsync(new CheckOtp { Id = user.Telephone, Otp = request.SmsCode }, cancellationToken);

                signature = request.SmsCode;
            }
            else if (request.SignType == SignType.Eds && request.HashCertificate is not null)
            {
                signature = request.HashCertificate;
            }
            else
            {
                _logger.LogWarning("Не выбран тип подписи");

                return new Result(InternalStatus.NotFound, "Неизвестные данные");
            }

            await _documentService.SignDocumentAsync(doc.TypeID, new DocumentSignInfo
            {
                DocFilePath = doc.FilePath,
                UserId = request.UserId,
                DocId = request.DocumentId,
                Signature = signature,
                SignType = request.SignType,
            }, cancellationToken);


            return new Result(InternalStatus.Success, "Документ подписан");
        }
    }
}
