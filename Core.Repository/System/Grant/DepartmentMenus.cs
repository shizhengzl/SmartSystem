using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 部门菜单表
    /// </summary>
    [Description("部门菜单表")]
    public class DepartmentMenus : SysBaseEntity
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        [Description("部门ID")]
        public Int64 DepartmentId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Description("菜单ID")]
        public Int64 MenuId { get; set; } 

        /// <summary>
        /// 单位ID
        /// </summary>
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }
    }
}
