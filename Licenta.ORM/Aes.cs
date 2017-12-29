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

        public byte[] Encrypt(byte[] input)
        {
            using (var algorithm = GetSymmetricAlgorithm())
            using (var encryptor = algorithm.CreateEncryptor(configuration.Key, configuration.Iv))            
            {
                return encryptor.TransformFinalBlock(input, 0, input.Length);
            }
        }

        public byte[] Decrypt(byte[] input)
        {
            using (var algorithm = GetSymmetricAlgorithm())
            using (var decryptor = algorithm.CreateDecryptor(configuration.Key, configuration.Iv))
            {
                return decryptor.TransformFinalBlock(input, 0, input.Length);                
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
