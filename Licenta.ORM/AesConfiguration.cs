using System;
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

        public static AesConfiguration FromFile()
        {
            return new AesConfiguration
            {
                BlockSize = 256,
                CipherMode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = Convert.FromBase64String("H+dCfDa4cugB2BPAA9S2lOe7cSnXJgKPqfiDehwak2w="),
                Iv = Convert.FromBase64String("dnyiUgfcGV9YBBafw4U3Cxqz4l6RlMI4s0pqVlWMuj8=")
            };
        }
    }
}
