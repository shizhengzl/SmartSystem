using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 单位菜单表
    /// </summary>
    [Description("单位菜单表")]
    public class CompanyMenus : SysBaseEntity
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Description("菜单ID")]
        public Int64 MenuId { get; set; }
    }
}
