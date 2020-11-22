using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    [Description("角色菜单表")]
    public class RoleMenus : SysBaseEntity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Description("角色ID")]
        public Int64 RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Description("菜单ID")]
        public Int64 MenuId { get; set; }
    }
}
