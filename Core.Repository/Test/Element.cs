using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace Core.Repository
{
    /// <summary>
    /// 页面元素
    /// </summary>
    [Description("页面元素")]
    public class Element : SysBaseEntity
    {
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public Int64 Sort { get; set; }

        /// <summary>
        /// 元素类型
        /// </summary>
        [Description("元素类型")]
        [Column(StringLength = 200)]
        public String Type { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        [Description("超时时间")]
        public Int64 Timeout { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Description("值")] 
        public String Value { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [Description("字段描述")]
        [Column(StringLength = 200)]
        public String Desc { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [Description("字段名称")]
        [Column(StringLength = 200)]
        public String Name { get; set; }
         

        /// <summary>
        /// 父级ID
        /// </summary>
        [Description("父级")]
        public Int64 ParentId { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        [Column(StringLength = 100)]
        [Description("父级名称")]
        public String ParentName { get; set; }



        /// <summary>
        /// 单位ID
        /// </summary>
        [Description("单位ID")]
        public Int64 CompanyId { get; set; }

    }
}
