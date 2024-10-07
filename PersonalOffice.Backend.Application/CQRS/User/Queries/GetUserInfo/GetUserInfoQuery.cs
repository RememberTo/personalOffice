using MediatR;
using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.User.Queries.GetUserInfo
{
    /// <summary>
    /// Контракт на получение информации о персоне
    /// </summary>
    public class GetUserInfoQuery : IRequest<UserInfoVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.UserAndContractRequest, MessageDataTypes";

        /// <summary>
        /// ID пользователя
        /// </summary>
        public required int UserID { get; set; }

        /// <summary>
        /// ID договора
        /// </summary>
        public int? ContractID { get; set; }
    }
}
