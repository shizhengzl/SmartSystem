using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
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
        public int Id { get; set; }
        /// <summary>
        /// 字符串StackTrace
        /// </summary>
        [Column(StringLength = -1)]
        public string StackTrace { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [Column(StringLength = -1)]
        public string Message { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [Column(StringLength = -1)]
        public string Description { get; set; }
        /// <summary>
        /// 操作日志
        /// </summary> 
        public DateTime CreateTime { get; set; }


    }
}
