using System.IO;
using System.Security.Cryptography;

namespace Licenta.ORM
{
    public class Aes
    {
        AesConfiguration configuration;

        public Aes(AesConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public byte[] Encrypt(byte[] plainTextBytes)
        {
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = configuration.BlockSize;
                symmetricKey.Mode = configuration.CipherMode;
                symmetricKey.Padding = configuration.Padding;
                using (var encryptor = symmetricKey.CreateEncryptor(configuration.Key, configuration.Iv))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    memoryStream.Close();
                    cryptoStream.Close();
                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] cipherTextBytesWithSaltAndIv)
        {
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = configuration.BlockSize;
                symmetricKey.Mode = configuration.CipherMode;
                symmetricKey.Padding = configuration.Padding;
                using (var decryptor = symmetricKey.CreateDecryptor(configuration.Key, configuration.Iv))
                using (var memoryStream = new MemoryStream(cipherTextBytesWithSaltAndIv))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    var plainTextBytes = new byte[cipherTextBytesWithSaltAndIv.Length];
                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    memoryStream.Close();
                    cryptoStream.Close();
                    return plainTextBytes;
                }
            }
        }
    }
}
