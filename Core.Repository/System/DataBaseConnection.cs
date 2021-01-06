using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
     
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    [Description("数据库连接字符串")]
    public class DataBaseConnection
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        [Description("主键")]
        public int Id { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        [Description("数据库名称")]
        public string DataBaseName { get; set; }


        /// <summary>
        /// 连接字符串
        /// </summary>
        [Column(StringLength = 2000)]
        [Description("连接字符串")]
        public string ConnectinString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [Description("数据库类型")]
        public DataType DataBaseType { get; set; }


        /// <summary>
        /// 单位ID
        /// </summary>
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }

    }
}
