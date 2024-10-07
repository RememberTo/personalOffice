using MediatR;
using PersonalOffice.Backend.Domain.Interfaces.Objects;

namespace PersonalOffice.Backend.Application.CQRS.Document.Commands.CreateInvestProfileDocument
{
    /// <summary>
    /// Контракт на создание документа инвест прфоиля
    /// </summary>
    public class CreateInvestProfileDocumentCommand : IRequest<IResult>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public int ContractId { get; set; }
        /// <summary>
        /// Параметры инвестиционного профиля
        /// </summary>
        public required IDictionary<string, string> Params { get; set; }
    }
}
