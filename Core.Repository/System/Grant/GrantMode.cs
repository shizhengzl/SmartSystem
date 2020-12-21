using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 授权模式
    /// </summary>
    public enum GrantMode
    {
        [Description("单位授权")]
        CompanyGrant = 0,
        [Description("角色授权")]
        RoleGrant = 1,
        [Description("部门授权")]
        DepartGrant = 2,
        [Description("用户授权")]
        UserGrant = 3
    }
}
