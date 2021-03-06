﻿using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    [Description("角色表")]
    public class Roles : SysBaseEntity
    {

        /// <summary>
        /// 单位ID
        /// </summary> 
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Column(StringLength = 100)]
        [Description("角色名称")]
        public string RoleName { get; set; }


        /// <summary>
        /// 角色描述
        /// </summary>
        [Column(StringLength = 100)]
        [Description("角色描述")]
        public string RoleDescription { get; set; }

    }
}
