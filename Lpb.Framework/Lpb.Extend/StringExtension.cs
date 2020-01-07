using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lpb.Extend
{
    public static class StringExtension
    {
        /// <summary>
        /// 把16进制的string转换成byte[]类型的数据
        /// </summary>
        /// <param name="hexString">16进制的string</param>
        /// <returns></returns>
        public static byte[] ConvertStringToByteArray(this string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)

                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return returnBytes;
        }

        /// <summary>
        /// 把byte[]类型的数据转换成16进制的string
        /// </summary>
        /// <param name="arry">byte数组</param>
        /// <returns></returns>
        public static string ConvertByteArrayToString(this byte[] arry)
        {
            string str = BitConverter.ToString(arry).Replace("-", string.Empty);

            return str;
        }

        /// <summary>
        /// 把byte类型的数据转换成16进制的string
        /// </summary>
        /// <param name="arry">byte</param>
        /// <returns></returns>
        public static string ConvertByteToHexString(this byte arry)
        {
            string str = BitConverter.ToString(new byte[] { arry }).Replace("-", string.Empty);
            //string str = arry.ToString("X2");

            return str;
        }

        //校验身份证号码是否合法
        public static bool CheckIDCardNumber(this string idCardNumber)
        {
            idCardNumber = idCardNumber.ToUpper().Trim();

            //正则验证
            Regex rg = new Regex(@"^\d{17}(\d|X)$");
            Match mc = rg.Match(idCardNumber);
            if (!mc.Success) return false;

            //加权码
            string code = idCardNumber.Substring(17, 1);
            double sum = 0;
            string checkCode = null;

            for (int i = 2; i <= 18; i++)
            {
                sum += int.Parse(idCardNumber[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2, i - 1) % 11);
            }

            string[] checkCodes = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            checkCode = checkCodes[(int)sum % 11];

            if (checkCode != code) return false;

            return true;
        }

    }
}
