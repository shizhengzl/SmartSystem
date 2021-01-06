using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository.Tools.Generator
{

    /// <summary>
    /// 模板管理
    /// </summary>
    [Description("模板管理")]
    public class CodeSnippet : SysBaseEntity
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        [Description("模板名称")]
        [Column(StringLength = 1000)]
        public string Name { get; set; }


        /// <summary>
        /// 模板内容
        /// </summary>
        [Description("模板内容")]
        [Column(StringLength = -1)] 
        public string Context { get; set; }


        /// <summary>
        /// 生成文件路劲
        /// </summary>
        [Description("生成文件路劲")]
        [Column(StringLength = 1000)]
        public string GeneratorPath { get; set; }


        /// <summary>
        /// 生成文件名
        /// </summary>
        [Description("生成文件路劲")]
        [Column(StringLength = 1000)]
        public string GeneratorFileName { get; set; }


        /// <summary>
        /// 帅选过滤数据
        /// </summary>
        [Description("帅选过滤数据")] 
        public Boolean IsFilter { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Description("是否启用")]
        public Boolean IsEnabled { get; set; }
    }
}
