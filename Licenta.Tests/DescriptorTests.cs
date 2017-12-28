using Microsoft.VisualStudio.TestTools.UnitTesting;
using Licenta.ORM;
using Licenta.Data;

namespace Licenta.Tests
{
    [TestClass]
    public class DescriptorTests
    {
        [TestMethod]
        public void CanDetectEncryptedProperties()
        {
            var encryptedAttributes = Reflection.GetEncryptedPropertyNames<Person>();
            CollectionAssert.AreEquivalent(new[] { "CardNo" }, encryptedAttributes);
        }
    }
}
