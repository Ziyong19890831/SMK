using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMK.Data.Utility
{
    public static class Cryptos
    {
        private static readonly string key = @"+{:>ZAQ!?><M0928";
        private static readonly string iv = @"+{:>ZAQ!?><M0928";

        public static string EncryptPwd(string plainText)
        {
            var plainBuffer = Encoding.UTF8.GetBytes(plainText);
            var result = string.Empty;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(plainBuffer))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    result = Convert.ToBase64String(resultStream.ToArray());
                }
            }

            return result;
        }
    }
}
