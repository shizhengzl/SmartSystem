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

namespace Core.Console
{

  
    class Program
    {
         
         
        static void Main(string[] args)
        {
            var snippet = "'\"+~+\"'";

            string input = "string sql = \"select left(isnull(max(SortCode),'000-'),4) as lastcode from TS_ScheduleRecord_Con where Id = \"+Id+\" and ContId = '\" + con_id + \"'\";";


            var space = "\\s'\\s\"\\s+";

            StringBuilder sb = new StringBuilder();

           


            string pattern = "(\\s*[']*\\s*\"\\s*\\+\\s*(\\w+)\\s*\\+\\s*\"\\s*[']*)";
            //Regex.Matches(input, "(\\s*)'(\\s*)\"(\\s*)+(\\s*)(.*)(\\s*)+")
            //MatchCollection col = Regex.Matches(input, pattern);

            //foreach (Match item in col)
            //{
            //    var s = item.Value;
            //}



            var formst = "string.Format(@ \" select *, (select count(*) from TM_ContractBillView where bill_id=bb.bill_id) as ischangehsl, (bill_hsl*bill_price)bill_hsjes,(bill_ysl*bill_price)bill_ysjes  from TM_ContractBill as bb where cont_id= { 0} order by bill_code asc\", querycondition);";


         
          
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
