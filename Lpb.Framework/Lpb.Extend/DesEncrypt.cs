using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lpb.Extend
{
    /// <summary>
    /// 可逆对称加密  密钥长度8
    /// </summary>
    public static class DesEncrypt
    {
        //8位长度
        private static string KEY = "LaibaJiankang";
        private static byte[] key = ASCIIEncoding.ASCII.GetBytes(KEY.Substring(0, 8));
        private static byte[] iv = ASCIIEncoding.ASCII.GetBytes(KEY.Insert(0, "Suoer").Substring(0, 8));

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string Encrypt(string strValue)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            using (memStream)
            {
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                StreamWriter sWriter = new StreamWriter(crypStream);
                sWriter.Write(strValue);
                sWriter.Flush();
                crypStream.FlushFinalBlock();
                memStream.Flush();
                return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="EncValue"></param>
        /// <returns></returns>
        public static string Decrypt(string EncValue)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            byte[] buffer = Convert.FromBase64String(EncValue);
            MemoryStream memStream = new MemoryStream();
            using (memStream)
            {
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                crypStream.Write(buffer, 0, buffer.Length);
                crypStream.FlushFinalBlock();
                return ASCIIEncoding.UTF8.GetString(memStream.ToArray());
            }
        }
    }
}
