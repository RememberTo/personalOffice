namespace PersonalOffice.Backend.Application.Common.Exceptions
{
    /// <summary>
    /// Ошибки связанные с сохранением файлов
    /// </summary>
    public class FileException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public FileException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public FileException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public FileException(string message, Exception ex) : base(message, ex) { }
    }
}
