using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    ///系统操作日志
    /// </summary>
    public class SystemLogs
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        [Description("主键")]
        public int Id { get; set; }
        /// <summary>
        /// 堆栈信息
        /// </summary>
        [Column(StringLength = -1)]
        [Description("堆栈信息")]
        public string StackTrace { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        [Column(StringLength = -1)]
        [Description("错误消息")]
        public string Message { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        [Column(StringLength = -1)]
        [Description("错误描述")]
        public string Description { get; set; }
        /// <summary>
        /// 异常时间
        /// </summary> 
        [Description("异常时间")]
        public DateTime CreateTime { get; set; }


    }
}
