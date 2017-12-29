using System.Security.Cryptography;

namespace Licenta.ORM
{
    public class AesConfiguration
    {
        public int BlockSize { get; set; }
        public CipherMode CipherMode { get; set; }
        public PaddingMode Padding { get; set; }
        public byte[] Key { get; set; }
        public byte[] Iv { get; set; }
    }
}
