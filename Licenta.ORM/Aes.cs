﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Licenta.ORM
{
    public class Aes
    {
        AesConfiguration configuration;

        public Aes(AesConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public byte[] Encrypt(byte[] plainTextBytes)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            using (var password = new Rfc2898DeriveBytes(configuration.Password, saltStringBytes, configuration.DerivationIterations))
            {
                var keyBytes = password.GetBytes(configuration.Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = configuration.BlockSize;
                    symmetricKey.Mode = configuration.CipherMode;
                    symmetricKey.Padding = configuration.Padding;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return cipherTextBytes;
                            }
                        }
                    }
                }
            }
        }

        public byte[] Decrypt(byte[] cipherTextBytesWithSaltAndIv)
        {
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(configuration.Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(configuration.Keysize / 8).Take(configuration.Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((configuration.Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((configuration.Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(configuration.Password, saltStringBytes, configuration.DerivationIterations))
            {
                var keyBytes = password.GetBytes(configuration.Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                Array.Resize(ref plainTextBytes, decryptedByteCount);
                                return plainTextBytes;
                            }
                        }
                    }
                }
            }
        }

        private byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
