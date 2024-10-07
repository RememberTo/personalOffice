using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace PersonalOffice.Backend.Application.CQRS.OneTimePass.Commands.SendOtp
{
    internal partial class GenerateOtpCommand
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.GenerateOtpRequest, MessageDataTypes";

        /// <summary>
        /// Идентификатор телефона
        /// </summary>
        public string? ID {get; set;}
        public int TTL { get; set; }

    }
}
