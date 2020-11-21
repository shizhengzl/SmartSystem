using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    public class Roles : SysBaseEntity
    {
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
