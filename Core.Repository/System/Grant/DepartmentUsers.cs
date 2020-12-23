using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 部门用户表
    /// </summary>
    [Description("部门用户表")]
    public class DepartmentUsers : SysBaseEntity
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        [Description("部门ID")]
        public Int64 DepartmentId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Description("用户ID")]
        public Int64 UserId { get; set; }


        /// <summary>
        /// 单位ID
        /// </summary>
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }
    }
}
