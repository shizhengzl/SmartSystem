using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 归类表
    /// </summary>
    [Description("归类表")]
    public class TableArea : SysBaseEntity
    {
        /// <summary>
        /// 连接字符串ID
        /// </summary>
        [Description("连接字符串ID")]
        public Int32 DabaBaseId { get; set; }


        /// <summary>
        /// 归类名称
        /// </summary>
        [Description("归类名称")]
        [Column(StringLength = 200)]
        public String AreaName { get; set; }
    }
}
