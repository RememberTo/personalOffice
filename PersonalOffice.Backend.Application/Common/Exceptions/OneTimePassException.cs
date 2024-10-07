namespace PersonalOffice.Backend.Application.Common.Exceptions
{
    /// <summary>
    /// Ошибки связанные с генерацией одноразовых кодов
    /// </summary>
    public class OneTimePassException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        public OneTimePassException()  { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public OneTimePassException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public OneTimePassException(string message, Exception ex) : base(message, ex) { }
    }
}
