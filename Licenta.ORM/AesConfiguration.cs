using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Licenta.ORM
{
    public class AesConfiguration
    {
        public string CertificateThumbprint { get; set; }
        public int BlockSize { get; set; }
        public CipherMode CipherMode { get; set; }
        public PaddingMode Padding { get; set; }
        public byte[] EncryptedKey { get; set; }
        public byte[] EncryptedIv { get; set; }
        public byte[] Key => Decrypt(EncryptedKey);
        public byte[] Iv => Decrypt(EncryptedIv);

        private byte[] Decrypt(byte[] input)
        {
            using (var certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certificateStore.Open(OpenFlags.ReadOnly);
                var certificate = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, CertificateThumbprint, false)[0];

                using (RSA rsa = certificate.GetRSAPrivateKey())
                {
                    return rsa.Decrypt(input, RSAEncryptionPadding.OaepSHA1);
                }
            }
        }

        private byte[] Encrypt(byte[] input)
        {
            using (var certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certificateStore.Open(OpenFlags.ReadOnly);
                var certificate = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, CertificateThumbprint, false)[0];

                using (RSA rsa = certificate.GetRSAPublicKey())
                {
                    return rsa.Encrypt(input, RSAEncryptionPadding.OaepSHA1);
                }
            }
        }

        public static AesConfiguration FromFile()
        {
            return new AesConfiguration
            {
                CertificateThumbprint = "9bbd172d892ea5f126afab5a7440aa5ae54eac87",
                BlockSize = 256,
                CipherMode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                EncryptedKey = Convert.FromBase64String("nX5RDbzhEQHdThBrCLR4WznVNLDhigMdaPYd9BVgcDpDFA0zlZqHm7UPF/rM/ITF9QA+e2IwKdvpOnTfcjU/qzsSkSSB/fwTgLv+glICV6ccoUWbKUAfRVseYlMiHqaC/2ad6DtEjqIJepcNjedGXLRUqs2etdnsLDfPLk3hJrnk6yk5fCHetUKpIFbrAgRG4/h9/WQ1BWR5C6O4Dl4MvhYrr1OAcB/MUWiY2cQhPoh1K840n5WYClmY+XRKJDGdICBg6lxuPskUMgkTyKgUgeIDg/Ze8q7BeZcGjdtSFVHDsU5Ush6XvI/9Uq9feZUvqVodsgCx/9cK07Y3fcpIxg=="),
                EncryptedIv = Convert.FromBase64String("mFkSHqtrPrAXvfKIMRfuT4p2vTSxjwDM145nCxZGzL35xjSfBa5kUsm28AhxdfGtoDG9CQyvhtnvIlGLujuw+Uj535A0xDBaiML3yjbWWTcODbxIpfGwzt7F4UFALe16k0+FNl9vlHXF9QNpNmZrEqXI7Yxi4ElOGzwArqCZ5KOaYFrupf8N1cYa4Te31PBMB+hy1idEBbZ/aW8afPz/DQ4GsXapypAKov66zvBY3QnXXlihyKx2xAYkd0q5x+t18cRC4L5reIETkxU5umqmH64aYi567gR7RiMXQU6qJGSvKb8R06IVeNqaGU5fqtVJE+esjUJsMUa/uGfDqG6ocg==")
            };
        }
    }
}
