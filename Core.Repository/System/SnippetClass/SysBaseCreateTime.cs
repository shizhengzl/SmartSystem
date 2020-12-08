using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    public class SysBaseCreateTime
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        [Description("主键")]
        public int Id { get; set; }



        /// <summary>
        /// 创建用户时间
        /// </summary>
        [Description("创建用户时间")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}
