using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
        public Int64 CreateUserName { get; set; }

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
        public Int64? ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        public DateTime? ModifyTime { get; set; }
    }
}
