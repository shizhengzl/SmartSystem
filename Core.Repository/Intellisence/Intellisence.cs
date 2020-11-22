using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 智能感知提示类
    /// </summary>
    [Description("代码片段表")]
    public class Intellisence
    { 
        /// <summary>
        /// 主键
        /// </summary>
        [Description("主键")]
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        /// <summary>
        /// 激活字符
        /// </summary>
        [Description("激活字符")]
        public string StartChar { get; set; }
        /// <summary>
        /// 显示字符
        /// </summary>
        [Description("显示字符")]
        [Column(StringLength = -1)]
        public string DisplayText { get; set; }
        /// <summary>
        /// 查询字符
        /// </summary>
        [Column(StringLength = -1)]
        [Description("查询字符")]
        public string SearchText { get; set; }
        /// <summary>
        /// 回车写入字符
        /// </summary>
        [Column(StringLength = -1)]
        [Description("回车写入字符")]
        public string InsertionText { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = -1)]
        [Description("描述")]
        public string Description { get; set; }
        /// <summary>
        /// 定义SQL
        /// </summary>
        [Column(StringLength = -1)]
        [Description("定义SQL")]
        public string DefinedSql { get; set; }
        /// <summary>
        /// SQL连接字符串
        /// </summary>
        [Column(StringLength = -1)]
        [Description("SQL连接字符串")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [Description("数据库类型")]
        public DataType DataType { get; set; }

        /// <summary>
        /// 是否SQL
        /// </summary>
        [Description("是否SQL")]
        public Boolean IsSql { get; set; }
    }
}
