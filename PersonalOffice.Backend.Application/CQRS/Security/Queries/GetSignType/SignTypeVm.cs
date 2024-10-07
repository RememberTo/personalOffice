using PersonalOffice.Backend.Domain.Enums;

namespace PersonalOffice.Backend.Application.CQRS.Security.Queries.GetSignType
{
    /// <summary>
    /// Модель представления типа подписи
    /// </summary>
    public class SignTypeVm
    {
        /// <summary>
        /// Тип подписи
        /// </summary>
        public SignType SignType { get; set; }
    }
}
