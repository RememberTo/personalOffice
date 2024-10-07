using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.Application.CQRS.Payment.Commands.TopUpAccount
{
    /// <summary>
    /// Модель представления результата операции
    /// </summary>
    public class TopUpVm
    {
        /// <summary>
        /// Сообщение о результате
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Тип операции
        /// </summary>
        public PaymentOptions PaymentOptions { get; set; }
        /// <summary>
        /// Файл с инструкциями к пополнению счета, может быть QR code или файл типа pdf в котором находятся реквиизиты
        /// </summary>
        public required string FileBase64 { get; set; }
    }
}
