using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 测试模块
    /// </summary>
    [Description("测试模块")]
    public class TestModule : SysBaseEntity
    {
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")] 
        public Int64 Description { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [Description("模块名称")]
        [Column(StringLength = 200)]
        public String ModuleName { get; set; }


        /// <summary>
        /// 模块描述
        /// </summary>
        [Description("模块描述")]
        [Column(StringLength = 200)]
        public String Node { get; set; }


        /// <summary>
        /// 子节点
        /// </summary> 
        [Description("子节点")]
        [Column(IsIgnore = true)]
        public List<TestModule> children { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        [Description("父级ID")]
        public Int64 ParentId { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        [Column(StringLength = 100)]
        [Description("父级名称")]
        public String ParentName { get; set; }
    }
}
