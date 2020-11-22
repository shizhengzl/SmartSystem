﻿using FreeSql.DataAnnotations;
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
        /// 如果离职情况请标记为禁用
        /// </summary> 
        [Description("启用")]
        public Boolean IsEnabled { get; set; } = true;

        
    }
}
