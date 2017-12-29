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
            var aes = new Aes();
            
            string plaintext = "plaintext";

            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] encryptedBytes = aes.Encrypt(plaintextBytes);

            byte[] decryptedBytes = aes.Decrypt(encryptedBytes);
            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            Assert.AreEqual(plaintext, decryptedString);
        }
    }
}
