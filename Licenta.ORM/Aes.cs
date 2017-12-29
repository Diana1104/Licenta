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
            using (var algorithm = GetSymmetricAlgorithm())
            using (var encryptor = algorithm.CreateEncryptor(configuration.Key, configuration.Iv))
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

        public byte[] Decrypt(byte[] cipherTextBytesWithSaltAndIv)
        {
            using (var algorithm = GetSymmetricAlgorithm())
            using (var decryptor = algorithm.CreateDecryptor(configuration.Key, configuration.Iv))
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

        private SymmetricAlgorithm GetSymmetricAlgorithm()
        {
            return new RijndaelManaged
            {
                BlockSize = configuration.BlockSize,
                Mode = configuration.CipherMode,
                Padding = configuration.Padding
            };
        }
    }
}
