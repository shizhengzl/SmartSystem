using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.UsuallyCommon
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 获取MD5得值，没有转换成Base64的
        /// </summary>
        /// <param name="sDataIn">需要加密的字符串</param>
        /// <param name="move">偏移量</param>
        /// <returns>sDataIn加密后的字符串</returns>
        public static string ToMD5(this string sDataIn, string move = "")
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] byt, bytHash;
            byt = System.Text.Encoding.UTF8.GetBytes(move + sDataIn);
            bytHash = md5.ComputeHash(byt);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
            }
            return sTemp;
        }

        /// <summary>
        /// 随机生成bool
        /// </summary>
        /// <returns></returns>
        public static bool RandomBool()
        { 
            return RamdoInt32() % 2 == 0; ;
        }

        /// <summary>
        /// 随机生成一个数字
        /// </summary>
        /// <param name="max">从0开始到max</param>
        /// <returns>INT32</returns>
        public static Int32 RamdoInt32(Int32 max = 9)
        {
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            return rd.Next(0, max);
        }

        /// <summary>
        /// 随机生成指定长度数字
        /// </summary>
        /// <param name="length">长度x</param>
        /// <param name="max">从0开始到max</param>
        /// <returns>string</returns>
        public static string RamdomString(Int32 length, Int32 max = 9)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.AppendFormat("{0}", RamdoInt32(max));
            }
            return sb.ToStringExtension();
        }

        /// <summary>
        /// 解决安全测评 Header Manipulation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToHeaderManipulation(this object obj)
        {
            string result = string.Empty;
            if (obj != null)
                result = result.ToStringExtension().Replace("\r", "").Replace("\n", "").Replace("\r\n", "").Replace("%0d", "").Replace("%0a", "");
            return result;
        }

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static System.Security.SecureString ToPasswordToSecureString(this string obj)
        {
            System.Security.SecureString ss = new System.Security.SecureString();
            obj.ToStringExtension().ToArray().ToList().ForEach(x =>
            {
                ss.AppendChar(x);
            });
            ss.MakeReadOnly();
            return ss;
        }


        /// <summary>
        /// 密码解密
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToSecureStringToPassword(this System.Security.SecureString obj)
        {
            string password = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(obj);
            try
            {
                password = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return password;
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String ToFirstCharToLower(this String str)
        {
            if (str.ToStringExtension().Length > 0)
                return str.Substring(0, 1).ToLower() + str.Substring(1);
            return str;
        }

    }
}
