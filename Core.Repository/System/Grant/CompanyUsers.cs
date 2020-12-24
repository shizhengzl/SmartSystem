using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 单位用户表
    /// </summary>
    [Description("单位用户表")]
    public class CompanyUsers : SysAuditEntity
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Description("用户ID")]
        public Int64 UserId { get; set; }

        /// <summary>
        /// 在职状态
        /// </summary>
        [Description("在职状态")]
        public JobStatus JobStatus { get; set; }


    }
}
