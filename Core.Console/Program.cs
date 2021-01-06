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
            List<SearchRule> response = new List<SearchRule>();

            var str = @"已出台CRM开发推广应用计划并发布，每天核查反馈进度情况
已完成初稿制定
跟石总沟通，暂无计划客户
团建办法、督导办法没有定稿，岗位职业考核周一督促人事组织
数据量较大，利用节假日完成
会议前策划书已提交
完成 完成
材料进销存和三大材料调差产品配置完毕。";
            List<string> keys = new List<string>() { "\r\n",  "\t", " " };
            keys.ForEach(x=>{
                SearchRule searchRule = new SearchRule() { SearchKey = x, SearchCount = 0 };
                str.GetStringMatchCount(searchRule);
                response.Add(searchRule);

             
            });

            var sp = response.OrderBy(o => o.SearchCount).FirstOrDefault();
            var spmax = response.OrderByDescending(o => o.SearchCount).FirstOrDefault();

            var rs = str.GetStringSingleColumn(new List<string>() { sp.SearchKey });
            rs.ForEach(x => {
                var columns = x.GetStringSingleColumn(new List<string>() { spmax.SearchKey });

                System.Console.WriteLine($"这一行共计：{columns.Count}列");
                columns.ForEach(p=> {
                    System.Console.WriteLine($"值：{p.ToStringExtension()}");
                });
             
            });

            //response.OrderByDescending(p => p.SearchCount).ToList().ForEach(o => {
            //    var s = o.SearchKey;
            //    var c = o.SearchCount;
            //    System.Console.WriteLine($"key:{o.SearchKey.ToString()}   count={o.SearchCount}");
            //});

            System.Console.ReadLine();
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
