using System.Security.Cryptography;
using System.Text;

namespace PersonalOffice.Backend.Application.Common.Global
{
    /// <summary>
    /// Вспомогательный класс для предоставления криптографических операций
    /// </summary>
    public class Crypto
    {
        /// <summary>
        /// Генерирует хэш для файлов по алгоритму HMACCHA1
        /// </summary>
        /// <param name="Code">Код подписи</param>
        /// <param name="Order">бинарный файл</param>
        /// <returns></returns>
        public static byte[] HmacSignature(string Code, byte[] Order)
        {
            using var HM = new HMACSHA1(Encoding.UTF8.GetBytes(Code));
            return HM.ComputeHash(Order);
        }
        /// <summary>
        /// Получить захешированную строку в SHA256
        /// </summary>
        /// <param name="bytes">хешируемые данные</param>
        /// <returns></returns>
        public static string GetStringSHA256Hash(byte[] bytes)
        {
            return GetHexStringFromByte(SHA256.HashData(bytes));
        }

        /// <summary>
        /// Получить строку HEX строку из байт
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetHexStringFromByte(byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (byte b in bytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}

