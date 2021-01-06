using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 表
    /// </summary>
    [Description("表")]
    public class Table
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        [Description("主键")]
        public Int64 Id { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        [Description("服务地址")]
        [Column(StringLength = 1000)]
        public string ServerAddress { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        [Description("数据库名称")]
        [Column(StringLength = 1000)]
        public string DataBaseName { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        [Description("表名")]
        [Column(StringLength = 1000)]
        public string TableName { get; set; }
        /// <summary>
        /// 表描述
        /// </summary>
        [Description("表描述")]
        [Column(StringLength = 1000)]
        public string TableDescription { get; set; }
    }
}
