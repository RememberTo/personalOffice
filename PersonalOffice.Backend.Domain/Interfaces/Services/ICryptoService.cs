namespace PersonalOffice.Backend.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с криптографией
    /// </summary>
    public interface IClsCryptoService
    {
        /// <summary>
        /// Кодирование
        /// </summary>
        /// <param name="strPlainText"></param>
        /// <returns></returns>
        public string Encrypt(string strPlainText);
        /// <summary>
        /// Декодирование
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public string Decrypt(string encryptedText);
    }
}
