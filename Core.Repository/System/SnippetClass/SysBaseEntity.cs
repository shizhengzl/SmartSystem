using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Core.UsuallyCommon;

namespace Core.Repository
{
    public class SysBaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        [Description("主键")]
        public Int64 Id { get; set; }


        /// <summary>
        /// 创建用户ID
        /// </summary>
        [Description("创建用户ID")]
        public Int64 CreateUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        [Description("创建用户")]
        [Column(StringLength = 100)]
        public String CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;


        /// <summary>
        /// 修改用户ID
        /// </summary>
        [Description("修改用户ID")]
        public Int64? ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        [Description("修改用户")]
        [Column(StringLength = 100)]
        public String ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        public DateTime? ModifyTime { get; set; }


        /// <summary>
        /// 设置默认值
        /// </summary> 
        /// <param name="currentUser"></param>
        public void SetCreateDefault(UserDto currentUser)
        {
            this.CreateUserId = currentUser.Id;
            this.CreateUserName = currentUser.UserName;
            this.CreateTime = System.DateTime.UtcNow;
        }

        /// <summary>
        /// 设置编辑默认值
        /// </summary>
        /// <param name="currentUser"></param>
        public void SetModifyDefault(UserDto currentUser)
        {
            this.ModifyUserId = currentUser.Id;
            this.ModifyUserName = currentUser.UserName;
            this.ModifyTime = System.DateTime.UtcNow;
        }
    }
}
