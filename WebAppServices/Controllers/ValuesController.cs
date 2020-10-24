using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Repository;
using Core.Services;
using Core.UsuallyCommon;
using Microsoft.AspNetCore.Mvc;
using WebAppServices.Model;


namespace WebAppServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private static List<String> GetFormat(string context)
        {
            string[] separatingChars = new string[] { ",",")","(",";"
            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();

        }
        private List<String> GetColumn(string context)
        {
            string[] separatingChars = new string[] { ".","+"," " , "@"
            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();

        }



        private List<String> GetValues(string context)
        {
            string[] separatingChars = new string[] { "+"," " , "@"
            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();

        }


        private string GeneratorParams(List<string> list)
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

        /// <summary>
        /// 根据类容字符串自动切割
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<String> GetStringSingleColumn(string context)
        {
            string[] separatingChars = new string[] { "\"", "'", ";",">=","<=" ,">"
                ,"<",",","[","]"

            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();
        }


        [HttpPost("ParserSQL")]
        public ResponseDto<string> ParserSQL([FromBody] RequestStringModel request)
        {
            var input = request.Input;
            ResponseDto<string> responsedto  = new ResponseDto<string>();
            var response = GetStringSingleColumn(input);
            StringBuilder sb = new StringBuilder();


            List<string> result = new List<string>();

            foreach (var item in response)
            {
                if (item.IndexOf('+') > 0 && item.Trim().Length > 1)
                    result.Add(item);
                sb.AppendLine(item);
            } 
            var lastresul = string.Empty; 
            List<String> gencol = new List<string>(); 
            for (int i = 0; i < result.Count; i++)
            {

                var columnname = GetColumn(result[i]).Where(x => x.IndexOf("(") == -1).LastOrDefault();
                gencol.Add(columnname);
                if (i + 1 == result.Count)
                {
                    input = input.Replace("'\"" + result[i] + "\"'", "@" + columnname);
                    input = input.Replace("\"" + result[i], "@" + columnname + "\"");
                }

                else
                {
                    input = input.Replace("'\"" + result[i] + "\"'", "@" + columnname);
                    input = input.Replace("\"" + result[i] + "\"", "@" + columnname);
                }
            }

            StringBuilder sbs = new StringBuilder();

            sbs.AppendLine(input);

            sbs.AppendLine(GeneratorParams(result));

            responsedto.Data = sbs.ToString();
            return responsedto;
        }


        [HttpPost("ParserSQLFormat")]
        public ResponseDto<string> ParserSQLFormat([FromBody] RequestStringModel request) {

            ResponseDto<string> responsedto = new ResponseDto<string>();

            var formst = request.Input;
            var lastindex = formst.LastIndexOf('"') + 2;
            StringBuilder sb = new StringBuilder();

            var splitarr = GetFormat(formst.Substring(lastindex)).ToList().Where(x => !string.IsNullOrEmpty(x)).ToList();
            string p = "('*{\\s*([\\d]+)\\s*}'*)";
            MatchCollection col = Regex.Matches(formst, p);

            var stringresult = formst.Substring(0, lastindex - 1) + ";";
            if (splitarr.Count == col.Count)
            {
                int i = 0;
                foreach (Match item in col)
                {
                    var s = item.Value;

                    var value = splitarr[i].ToStringExtension().Trim();
                    var columnname = GetColumn(value).Where(x => x.IndexOf("(") == -1).LastOrDefault();
                    stringresult = stringresult.Replace(s, "@" + columnname);
                    i++;
                }

                stringresult = Regex.Replace(stringresult, "String.Format\\s*\\(\\s*@\\s*", "", RegexOptions.IgnoreCase);


                sb.AppendLine(stringresult);
                sb.AppendLine(GeneratorParams(splitarr));

                responsedto.Data = sb.ToString();
            }


            return responsedto;
        }


    }
}
