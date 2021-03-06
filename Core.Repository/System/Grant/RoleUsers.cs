﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace Core.Repository
{
    /// <summary>
    /// 角色用户表
    /// </summary>
    [Description("角色用户表")]
    public class RoleUsers : SysBaseEntity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Description("角色ID")]
        public Int64 RoleId { get; set; }

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
