using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Intellisence
{
    /// <summary>
    /// 智能感知提示数据类
    /// </summary>
    public class SCompletionList
    {
        /// 显示字符
        /// </summary>
        public string DisplayText { get; set; }
        /// <summary>
        /// 回车写入字符
        /// </summary>
        public string InsertionText { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
