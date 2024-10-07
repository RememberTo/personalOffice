namespace PersonalOffice.Backend.Application.Common.Exceptions
{
    /// <summary>
    /// ИСключения для неправильных запросов
    /// </summary>
    public class BadRequestException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public BadRequestException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public BadRequestException(string message, Exception ex) : base(message, ex) { }
    }
}
