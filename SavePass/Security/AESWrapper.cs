using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SavePass.Security
{
    public class AESWrapper
    {
        private readonly byte[] salt = { 250, 18, 32, 56, 232, 245, 69, 87 };
        private readonly Encoding defaultEncoding = Encoding.UTF8;

        private readonly int keySize;
        private readonly int blockSize;

        public AESWrapper(string password, int keySize = 256, int blockSize = 128)
        {
            this.keySize = keySize;
            this.blockSize = blockSize;

            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password ?? String.Empty, salt))
            {
                Key = rfc2898DeriveBytes.GetBytes(keySize / 8);
                IV = rfc2898DeriveBytes.GetBytes(blockSize / 8);
            }
        }

        public byte[] Key { get; }

        public byte[] IV { get; }

        public byte[] Encrypt(string data) => Encrypt(data, defaultEncoding);

        public byte[] Encrypt(string data, Encoding encoding)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    rijndael.KeySize = keySize;
                    rijndael.BlockSize = blockSize;
                    rijndael.Key = Key;
                    rijndael.IV = IV;
                    rijndael.Mode = CipherMode.CBC;

                    using (ICryptoTransform encryptor = rijndael.CreateEncryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor,
                        CryptoStreamMode.Write))
                    {
                        byte[] bytes = encoding.GetBytes(data);
                        cryptoStream.Write(bytes, 0, bytes.Length);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        public string Decrypt(byte[] data) => Decrypt(data, defaultEncoding);

        public string Decrypt(byte[] data, Encoding encoding)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    rijndael.KeySize = 256;
                    rijndael.BlockSize = 128;
                    rijndael.Key = Key;
                    rijndael.IV = IV;
                    rijndael.Mode = CipherMode.CBC;

                    using (ICryptoTransform decryptor = rijndael.CreateDecryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor,
                        CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                    }

                    return encoding.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}