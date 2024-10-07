using MediatR;
using Microsoft.Extensions.Logging;
using PersonalOffice.Backend.Application.Common.Exceptions;
using PersonalOffice.Backend.Application.Common.Extesions;
using PersonalOffice.Backend.Application.Common.Global;
using PersonalOffice.Backend.Domain.Entites.SQL;
using PersonalOffice.Backend.Domain.Entites.Transport;
using PersonalOffice.Backend.Domain.Entities.Payment;
using PersonalOffice.Backend.Domain.Entities.Report;
using PersonalOffice.Backend.Domain.Enums;
using PersonalOffice.Backend.Domain.Interfaces.Services;
using QRCoder;
using System.Drawing;
using Convert = PersonalOffice.Backend.Application.Common.Global.Convert;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Commands.TopUpAccount
{
    internal class TopUpAccountCommandHandler(
        ILogger<TopUpAccountCommandHandler> logger,
        ITransportService transportService,
        IContractService contractService) : IRequestHandler<TopUpAccountCommand, TopUpVm>
    {
        private readonly ILogger<TopUpAccountCommandHandler> _logger = logger;
        private readonly ITransportService _transportService = transportService;
        private readonly IContractService _contractService = contractService;

        public async Task<TopUpVm> Handle(TopUpAccountCommand request, CancellationToken cancellationToken)
        {
            await _contractService.CheckContract(request.ContractId, request.UsertId, cancellationToken);

            return request.PaymentOptions switch
            {
                PaymentOptions.QRCode or PaymentOptions.BankDetails =>
                await GetPaymentPaper(request.PaymentOptions, new NspkPaymentRequest()
                {
                    ContractID = request.ContractId,
                    PortfolioID = request.PortfolioId,
                    Amount = request.Quantity,
                }, cancellationToken),

                PaymentOptions.SBP =>
                await RegisterPayment(request.IsMobile, new NspkPaymentRequest()
                {
                    ContractID = request.ContractId,
                    PortfolioID = request.PortfolioId,
                    Amount = request.Quantity,
                    Operator = NspkOperator.Rosbank
                }, cancellationToken),

                _ => throw new NotFoundException("Отсутвует тип операции"),
            };
        }

        private async Task<TopUpVm> RegisterPayment(bool isMobile, NspkPaymentRequest request, CancellationToken cancellationToken)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.NSPK,
                Method = "RegisterPayment",
                Data = request
            }, TimeSpan.FromMinutes(4));

            if (msg.Data.IsException(out Exception ex))
            {
                _logger.LogError("Внутренняя ошибка: {errmsg}", ex.Message);
                throw new InvalidOperationException(ex.Message);
            }

            var nspkResult = Convert.DataTo<NspkPaymentResponse>(msg.Data);

            if (nspkResult.Payload is null) throw new InvalidOperationException("Отсутсвуют данные, попробуйте позже");

            return new TopUpVm
            {
                Message = "QR Code сформирован",
                PaymentOptions = PaymentOptions.SBP,
                FileBase64 = isMobile 
                    ? GetQrCodeBase64(nspkResult.Payload)
                    : nspkResult.Payload,
             };
        }

        private async Task<TopUpVm> GetPaymentPaper(PaymentOptions method, NspkPaymentRequest request, CancellationToken cancellationToken)
        {
            var msg = await _transportService.RPCServiceAsync(new Message
            {
                Source = MicroserviceNames.Backend,
                Destination = MicroserviceNames.DbConnector,
                Method = "NSPK_GetPaymentPaper",
                Data = request
            }, cancellationToken);

            var sqlResult = Convert.DataTo<SQLOperationResult<string>>(msg.Data);

            if (!sqlResult.Success || sqlResult.ReturnValue is null)
            {
                _logger.LogError("Ошибка при запросе NSPK_GetPaymentPaper: {msg}", sqlResult.Message);
                throw new InvalidOperationException(sqlResult.Message);
            }

            if (method == PaymentOptions.BankDetails)
            {
                string[] payloadParam = sqlResult.ReturnValue.Split('|');
                var param = new Dictionary<string, string>();
                foreach (var q in payloadParam)
                {
                    string[] keyValue = q.Split('=');
                    if (keyValue.Length > 1)
                    {
                        param.Add(keyValue[0], keyValue[1]);
                    }
                }

                var msgToReport = await _transportService.RPCServiceAsync(new Message
                {
                    Source = MicroserviceNames.Backend,
                    Destination = MicroserviceNames.ReportMaster,
                    Method = "GetPdfWithQrCodeForPaymentByDetails_RP",
                    Data = new PaymentByDetailsRequest()
                    {
                        ContractID = request.ContractID.ToString(),
                        Amount = request.Amount.ToString(),
                        BankName = param["BankName"],
                        BIC = param["BIC"],
                        CorrespAcc = param["CorrespAcc"],
                        KPP = param["KPP"],
                        Name = param["Name"],
                        PayeeINN = param["PayeeINN"],
                        PersonalAcc = param["PersonalAcc"],
                        Purpose = param["Purpose"],
                        QrCode = GetQrCodeBase64(sqlResult.ReturnValue),
                    }
                }, TimeSpan.FromMinutes(2));

                var result = Convert.DataTo<ObjectBytes>(msgToReport.Data);

                return new TopUpVm { Message = "PDF файл сформирован", PaymentOptions = PaymentOptions.BankDetails, FileBase64 = result.Value };
            }
            else
            {
                return new TopUpVm { Message = "QR Code сформирован", PaymentOptions = PaymentOptions.QRCode, FileBase64 = GetQrCodeBase64(sqlResult.ReturnValue) };
            }
        }

        private string GetQrCodeBase64(string payload)
        {
            var foreground = Color.FromArgb(255, 99, 00, 47).ColorToByteArray();
            var background = Color.White.ColorToByteArray();

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.L);
            using var qrCode = new PngByteQRCode(qrCodeData);

            byte[] qrCodeImage = qrCode.GetGraphic(5, foreground, background, false);

            return System.Convert.ToBase64String(qrCodeImage);
        }
    }
}
