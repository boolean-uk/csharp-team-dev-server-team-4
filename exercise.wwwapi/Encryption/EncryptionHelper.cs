using System.Security.Cryptography;
using System.Text;

namespace exercise.wwwapi.Encryption
{
    public class EncryptionHelper
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
    }

    public EncryptionHelper(IConfiguration config)
        {
            _key = Convert.FromBase64String(config["Encryption:Key"] ?? throw new InvalidOperationException("Encryption key not found in configuration."));
            _iv = Convert.FromBase64String(config["Encryption:IV"] ?? throw new InvalidOperationException("Encryption IV not found in configuration."));
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            var buffer = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}