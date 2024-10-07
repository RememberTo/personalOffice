using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.User.Commands.UpdateUserProfile
{
    internal class SetTwoFactAuthCommand
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.PO_TwoFactAuthRequest, MessageDataTypes";
        [JsonProperty("UserID")]
        public int UserId { get; set; }
        [JsonProperty("TwoFactAuth")]
        public bool IsTwoFactAuth { get; set; }
    }
}
