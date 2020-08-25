using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 智能感知提示类
    /// </summary>
    public class Intellisence
    { 
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        /// <summary>
        /// 激活字符
        /// </summary>
        public string StartChar { get; set; }
        /// <summary>
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
        /// <summary>
        /// 定义SQL
        /// </summary>
        public string DefinedSql { get; set; }
        /// <summary>
        /// SQL连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// 是否SQL
        /// </summary>
        public Boolean IsSql { get; set; }
    }
}
