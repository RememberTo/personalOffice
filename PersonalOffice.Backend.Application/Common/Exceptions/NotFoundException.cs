namespace PersonalOffice.Backend.Application.Common.Exceptions
{
    /// <summary>
    /// Исключение для не найденных данных
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NotFoundException(string message) : base(message)  {  }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public NotFoundException(string message, Exception ex) : base(message, ex) { }
    }
}
