using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    [Description("用户表")]
    public class Users : SysBaseEntity
    {
       
        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 100)]
        [Description("用户名")]
        public string UserName { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        [Column(StringLength = 100)]
        [Description("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column(StringLength = 100)]
        [Description("昵称")]
        public string NikeName { get; set; }
        
        /// <summary>
        /// 手机号码
        /// </summary>
        [Column(StringLength = 20)]
        [Description("手机号码")]
        public string Phone { get; set; }



        /// <summary>
        /// 密码
        /// </summary>
        [Column(StringLength = 100)]
        [Description("密码")]
        public string Password { get; set; }


        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(StringLength = 200)]
        [Description("邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 启用
        /// </summary> 
        [Description("启用")]
        public Boolean IsEnabled { get; set; } = true;


        /// <summary>
        /// 单位ID
        /// </summary> 
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }



        /// <summary>
        /// 部门ID
        /// </summary> 
        [Description("部门ID")]
        [Column(IsIgnore = true)]
        public List<Int64> DepartmentId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary> 
        [Description("角色ID")]
        [Column(IsIgnore = true)]
        public List<Int64> RoleId { get; set; }



    }
}
