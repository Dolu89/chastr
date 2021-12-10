using NBitcoin;
using NBitcoin.DataEncoders;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Chastr.Utils
{
    /// <summary>
    /// Custom code from https://gist.github.com/mhingston/a47caa21298950abc4d8422d98b7437e
    /// </summary>
    public class AES
    {
        private static byte[] GetSharedPubKey(string privateKey, string publicKey)
        {
            var pubKey = new PubKey(publicKey);
            var key = new Key(Encoders.Hex.DecodeData(privateKey));
            return pubKey.GetSharedPubkey(key).ToBytes().Skip(1).ToArray();
        }

        public static (string cipherText, string iv) Encrypt(string plainText, string privateKey, string publicKey)
        {
            byte[] cipherData;
            Aes aes = Aes.Create();
            aes.Key = GetSharedPubKey(privateKey, publicKey);
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            ICryptoTransform cipher = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, cipher, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }

                cipherData = ms.ToArray();
            }

            return (Convert.ToBase64String(cipherData), Convert.ToBase64String(aes.IV));
        }

        public static string Decrypt(string cipherText, string iv, string privateKey, string publicKey)
        {
            string plainText;
            Aes aes = Aes.Create();
            aes.Key = GetSharedPubKey(privateKey, publicKey);
            aes.IV = Convert.FromBase64String(iv);
            aes.Mode = CipherMode.CBC;
            ICryptoTransform decipher = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream cs = new CryptoStream(ms, decipher, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        plainText = sr.ReadToEnd();
                    }
                }

                return plainText;
            }
        }
    }
}
