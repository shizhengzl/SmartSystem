using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    public class TreeModel<T>
    {
        /// <summary>
        /// 子节点
        /// </summary> 
        [Description("子节点")]
        public List<T> children { get; set; }

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
