using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 归类数据表
    /// </summary>
    [Description("归类数据表")]
    public class TableAreaData : SysBaseEntity
    {
        /// <summary>
        /// 归类表ID
        /// </summary>
        [Description("归类表ID")]
        public Int32 TableAreaId { get; set; }


        /// <summary>
        /// 连接字符串ID
        /// </summary>
        [Description("连接字符串ID")]
        public Int32 DabaBaseId { get; set; }


        /// <summary>
        /// 数据库
        /// </summary>
        [Description("数据库")]
        [Column(StringLength = 200)]
        public String DataBaseName { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        [Description("表名")]
        [Column(StringLength = 200)]
        public String TableName { get; set; }
    }
}
