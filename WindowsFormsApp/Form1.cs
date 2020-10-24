using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnexec_Click(object sender, EventArgs e)
        {
            var input = txtInput.Text; 
            var response = GetStringSingleColumn(input); 
            txtoutput.Text = GeneratorParams(response);
        }


        public string GeneratorParams(List<string> list)
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
        public List<String> GetStringSingleColumn(string context)
        {
            string[] separatingChars = new string[] { "\"", "'", ";",">=","<=" ,">"
                ,"<",",","[","]"

            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>(); 
        }

        private void btnsql_Click(object sender, EventArgs e)
        {
            var input = txtInput.Text;

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
                if (i+ 1 == result.Count)
                {
                    input = input.Replace("'\"" + result[i] + "\"'", "@" + columnname );
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
             
            txtoutput.Text = sbs.ToString();
        } 


        public List<String> GetColumn(string context)
        {
            string[] separatingChars = new string[] { ".","+"," " , "@"
            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();
             
        }


        public List<String> GetValues(string context)
        {
            string[] separatingChars = new string[] { "+"," " , "@"
            };
            string[] linedatas = context.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            return linedatas.ToList<string>();

        }
    }
}
