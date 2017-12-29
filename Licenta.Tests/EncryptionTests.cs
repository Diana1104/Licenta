using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Licenta.ORM;
using System;

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
                Key = Convert.FromBase64String("H+dCfDa4cugB2BPAA9S2lOe7cSnXJgKPqfiDehwak2w="),
                Iv = Convert.FromBase64String("dnyiUgfcGV9YBBafw4U3Cxqz4l6RlMI4s0pqVlWMuj8=")
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
