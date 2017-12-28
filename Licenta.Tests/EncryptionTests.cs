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
            var aes = new Aes();

            string password = "123";
            string plaintext = "plaintext";

            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] encryptedBytes = aes.Encrypt(plaintextBytes, password);

            byte[] decryptedBytes = aes.Decrypt(encryptedBytes, password);
            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            Assert.AreEqual(plaintext, decryptedString);
        }
    }
}
