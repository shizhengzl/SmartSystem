using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 用户菜单表
    /// </summary>
    [Description("用户菜单表")]
    public class UserMenus : SysBaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Description("用户ID")]
        public Int64 UserId { get; set; }

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
