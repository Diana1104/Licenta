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
            var descriptor = new Descriptor();
            var encryptedAttributes = descriptor.GetEncryptedPropertyNames<Person>();
            CollectionAssert.AreEquivalent(new[] { "CardNo" }, encryptedAttributes);
        }
    }
}
