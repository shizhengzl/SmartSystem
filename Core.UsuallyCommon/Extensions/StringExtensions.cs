using System;
using System.Collections.Generic;
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
    }
}
