namespace PersonalOffice.Backend.Domain.Entities.Mail
{
    /// <summary>
    /// Ответ об отправке сообщения
    /// </summary>
    public class MailResponse
    {
        /// <summary>
        /// Идентификатор запроса
        /// </summary>
        public string RequestID = "";
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public int MailID;
    }
}
