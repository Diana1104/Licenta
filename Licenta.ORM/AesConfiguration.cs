using System.IO;
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
        public byte[] Key => Decrypt(EncryptedKey, CertificateThumbprint);
        public byte[] Iv => Decrypt(EncryptedIv, CertificateThumbprint);

        private static byte[] Decrypt(byte[] input, string certificateThumbprint)
        {
            using (var certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certificateStore.Open(OpenFlags.ReadOnly);
                var certificate = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false)[0];

                using (RSA rsa = certificate.GetRSAPrivateKey())
                {
                    return rsa.Decrypt(input, RSAEncryptionPadding.OaepSHA1);
                }
            }
        }

        private static byte[] Encrypt(byte[] input, string certificateThumbprint)
        {
            using (var certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certificateStore.Open(OpenFlags.ReadOnly);
                var certificate = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false)[0];

                using (RSA rsa = certificate.GetRSAPublicKey())
                {
                    return rsa.Encrypt(input, RSAEncryptionPadding.OaepSHA1);
                }
            }
        }

        public static AesConfiguration FromFile()
        {
            if (File.Exists("crypto.config"))
            {
                return XmlSerialization.Deserialize<AesConfiguration>(File.ReadAllBytes("crypto.config"));
            }
            else
            {
                string certificateThumbprint = "9bbd172d892ea5f126afab5a7440aa5ae54eac87";

                var config = new AesConfiguration
                {
                    CertificateThumbprint = certificateThumbprint,
                    BlockSize = 256,
                    CipherMode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    EncryptedKey = Encrypt(GetRandomBytes(32), certificateThumbprint),
                    EncryptedIv = Encrypt(GetRandomBytes(32), certificateThumbprint)
                };

                byte[] bytes = XmlSerialization.Serialize(config);
                File.WriteAllBytes("crypto.config", bytes);

                return config;
            }
        }

        private static byte[] GetRandomBytes(int count)
        {
            byte[] randomBytes = new byte[count];
            var randomBytesGenerator = new RNGCryptoServiceProvider();
            randomBytesGenerator.GetNonZeroBytes(randomBytes);
            return randomBytes;
        }
    }
}
