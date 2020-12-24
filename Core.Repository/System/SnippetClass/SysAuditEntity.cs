using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    public class SysAuditEntity : SysBaseEntity
    {
        /// <summary>
        /// 审核用户ID
        /// </summary>
        [Description("审核用户ID")]
        public Int64 AuditUserId { get; set; } 

        /// <summary>
        /// 审核用户
        /// </summary>
        [Description("审核用户")]
        [Column(StringLength = 100)]
        public Int64 AuditUserName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Description("审核时间")]
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        [Description("审核备注")]
        [Column(StringLength = -1)]
        public Int64 AuditNote { get; set; }

        /// <summary>
        /// 文件组ID
        /// </summary>
        [Description("文件组ID")]
        public Guid? FileGroupId { get; set; }

    }
}
