using Core.Repository.Generator;
using Core.Repository;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.UsuallyCommon;
using Core.Services;
using System.Text.RegularExpressions;
using System.Security;

namespace Core.Console
{
    public static class Extenstion
    {
        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static SecureString PasswordToSecureString(this string obj)
        {
            System.Security.SecureString ss = new System.Security.SecureString();
            obj.ToStringExtension().ToArray().ToList().ForEach(x => {
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
        public static String SecureStringToPassword(this System.Security.SecureString obj)
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


    }

    class Program
    {
         
         
        static void Main(string[] args)
        {

            var pwd = "shizheng";

            var s = pwd.PasswordToSecureString();

            var n = s.SecureStringToPassword();

            var a = n;
        }




       

        private static List<String> GetValues(string context)
        {
            string[] separatingChars = new string[] { "+"," " , "@"
            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();

        }
        private static string GeneratorParams(List<string> list)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SqlParameter[] param = new SqlParameter[] { ");

            var snippt = " new SqlParameter(\"@@ColumnName\",@ColumnValue)";
            var i = 0;
            foreach (var item in list)
            {
                var name = GetColumn(item).Where(x => x.IndexOf('(') == -1).LastOrDefault();
                var it = GetValues(item).LastOrDefault();

                if (i == 0)
                    sb.AppendLine(snippt.Replace("@ColumnName", name).Replace("@ColumnValue", it));
                else
                    sb.AppendLine("," + snippt.Replace("@ColumnName", name).Replace("@ColumnValue", it));
                i++;
            }

            sb.AppendLine("};");

            return sb.ToString();

        }

        private static List<String> GetFormat(string context)
        {
            string[] separatingChars = new string[] { ",",")","(",";"
            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();

        }

        private static List<String> GetColumn(string context)
        {
            string[] separatingChars = new string[] { ".","+"," " , "@"
            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();

        }
    }

    
 
}
