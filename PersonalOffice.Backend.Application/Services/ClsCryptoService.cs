using PersonalOffice.Backend.Domain.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace PersonalOffice.Backend.Application.Services
{
    internal class ClsCryptoService : IClsCryptoService
    {
        private readonly Aes _myAes;
        private readonly int _iterations;
        private readonly byte[] _salt;

        public ClsCryptoService(string secretKey, string ivKey, string saltKey)
        {
            _myAes = Aes.Create();
            _myAes.BlockSize = 128;
            _myAes.KeySize = 128;
            _myAes.IV = HexStringToByteArray(ivKey);

            _myAes.Padding = PaddingMode.PKCS7;
            _myAes.Mode = CipherMode.CBC;
            _iterations = 1000;
            _salt = Encoding.UTF8.GetBytes(saltKey);
            _myAes.Key = GenerateKey(secretKey);
        }

        public string Encrypt(string strPlainText)
        {
            var strText = Encoding.UTF8.GetBytes(strPlainText);
            ICryptoTransform transform = _myAes.CreateEncryptor();
            var cipherText = transform.TransformFinalBlock(strText, 0, strText.Length);

            return Convert.ToBase64String(cipherText);
        }

        public string Decrypt(string encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptor = _myAes.CreateDecryptor(_myAes.Key, _myAes.IV);
            var originalBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(originalBytes);
        }

        public static byte[] HexStringToByteArray(string strHex)
        {
            var r = new byte[strHex.Length / 2];
            for (int i = 0; i <= strHex.Length - 1; i += 2)
            {
                r[i / 2] = Convert.ToByte(Convert.ToInt32(strHex.Substring(i, 2), 16));
            }
            return r;
        }

        private byte[] GenerateKey(string strPassword)
        {
            var rfc2898 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(strPassword), _salt, _iterations, HashAlgorithmName.SHA256);

            return rfc2898.GetBytes(128 / 8);
        }
    }
}
