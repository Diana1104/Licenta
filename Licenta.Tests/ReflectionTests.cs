using Microsoft.VisualStudio.TestTools.UnitTesting;
using Licenta.ORM;
using Licenta.Data;

namespace Licenta.Tests
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        public void CanDetectEncryptedProperties()
        {
            Assert.IsTrue(Reflection.IsEncrypted<Person>("CardNo"));
        }

        [TestMethod]
        public void CanDetectNonEncryptedProperties()
        {
            Assert.IsFalse(Reflection.IsEncrypted<Person>("FirstName"));
        }
    }
}
