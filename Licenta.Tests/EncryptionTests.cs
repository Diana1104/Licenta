using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Licenta.ORM;

namespace Licenta.Tests
{
    [TestClass]
    public class EncryptionTests
    {
        [TestMethod]
        public void CanEncryptAndDecryptSuccessfully()
        {
            var aesConfiguration = new AesConfiguration()
            {
                Keysize = 256,
                DerivationIterations = 1000,
                BlockSize = 256,
                CipherMode = System.Security.Cryptography.CipherMode.CBC,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7,
                Password = "123"
            };

            var aes = new Aes(aesConfiguration);
            
            string plaintext = "plaintext";

            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] encryptedBytes = aes.Encrypt(plaintextBytes);

            byte[] decryptedBytes = aes.Decrypt(encryptedBytes);
            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            Assert.AreEqual(plaintext, decryptedString);
        }
    }
}
