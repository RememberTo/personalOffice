using MediatR;
using Newtonsoft.Json;

namespace PersonalOffice.Backend.Application.CQRS.File.Queries
{
    /// <summary>
    /// Контракт на получение файла уведомления по идентификатору 
    /// </summary>
    public class GetNotifyFileByIdQuery : IRequest<FileVm>
    {
        [JsonProperty("$type")]
        private string deserizlizeType => "MessageDataTypes.PONotifyFileForUserIDRequest, MessageDataTypes";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public int FileId { get; set; }

    }
}
