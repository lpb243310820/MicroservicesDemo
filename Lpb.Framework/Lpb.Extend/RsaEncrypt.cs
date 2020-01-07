using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Lpb.Extend
{
    public class RsaEncrypt
    {
        private static string PublicKey = @"BgIAAACkAABSU0ExAAQAAAEAAQDlrlvIesvhOZxK8dVyVMC+HUoB7FchNRkrUgJSK6FlfPpnoEPGq7bbO2FxebuFE331gNugopwq+82cylsQycSwFBBFbQTh+X9hmW2fOReRoCxDsGdlscm06BX/26fHqO09RoCpbTE+NDpe6PmNMoIopcvrLU5WeLD/EIsjFyvNvg==";
        private static string PrivateKey = @"BwIAAACkAABSU0EyAAQAAAEAAQDlrlvIesvhOZxK8dVyVMC+HUoB7FchNRkrUgJSK6FlfPpnoEPGq7bbO2FxebuFE331gNugopwq+82cylsQycSwFBBFbQTh+X9hmW2fOReRoCxDsGdlscm06BX/26fHqO09RoCpbTE+NDpe6PmNMoIopcvrLU5WeLD/EIsjFyvNvv28ikuyuQa95UBe5VNy/1HDrTo7BZYClfLULVVS/tAt/tqVMXS6p9JlFWR6SbrJUeR0PGFmLOdpj1NvkCVNrOMJUlrrFcGTnf+G8dKFxsqWZ9cmKffYMfIHVt1xqK6fkou4l1nA5z0gs/IAFvvmWyT3QQV1kQhWl21b7sCfdIrWZWlyB92wtMbGGbwJiwRDnCJ7gdt37wW3XSjSMDQqyGuNfnTM1XgCWzMTHNnFUqT35OUj5twmSSrqkbaZLxfElbm8TjBwudQDpgivvil396QH4dzo6r6NTU+/LDqzjtIv1DN31bhZhq9+Yezoxd8BSKiKMDwUZYFN/ztmtNDnO7gisZPy1ZGvltWuVCkpavWB6fNrZmqg5LcFH46vGmE4GR5CXg5PLMrlC0+97lpGLlU9XaB3kbXOQ1n9XfPkvJh+gdyXXcfymN3DAHRk352VlyexKkN+CBHzGEmzvVLqPG3DAvJnmHx0D+leLlxg8xp04CjkAL+rPk1+yhB6WMlU9I4sTyBPkmpOv188NC52JO/fSLd/ZuIf1NsceO4vLywbPskoQiVKn6PDzX0R6ztPvYvCON45GZ+NykzATXnczgM=";


        /// <summary>
        /// 获取密钥对   key公钥  value私鈅
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, string> GetKeyPair()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKey = Convert.ToBase64String(rsa.ExportCspBlob(false));
            string privateKey = Convert.ToBase64String(rsa.ExportCspBlob(true));
            return new KeyValuePair<string, string>(publicKey, privateKey);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="publicKey">为空使用默认公钥</param>
        /// <returns></returns>
        public static string Encrypt(string content, string publicKey = "")
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] DataToEncrypt = ByteConverter.GetBytes(content);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            publicKey = string.IsNullOrEmpty(publicKey) ? PublicKey : publicKey;
            byte[] publicKeyBytes = Convert.FromBase64String(publicKey);
            rsa.ImportCspBlob(publicKeyBytes);//公钥加密
            byte[] resultBytes = rsa.Encrypt(DataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="privateKey">为空使用默认私鈅</param>
        /// <returns></returns>
        public static string Decrypt(string content, string privateKey = "")
        {
            byte[] dataToDecrypt = Convert.FromBase64String(content);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            privateKey = string.IsNullOrEmpty(privateKey) ? PrivateKey : privateKey;
            byte[] privateKeyBytes = Convert.FromBase64String(privateKey);
            RSA.ImportCspBlob(privateKeyBytes);
            byte[] resultBytes = RSA.Decrypt(dataToDecrypt, false);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetString(resultBytes);
        }
    }
}
