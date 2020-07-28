using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    /// <summary>
    /// 属性类
    /// </summary>
    public class CsharpProperty
    {

        /// <summary>
        /// 属性类型
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 属性描述
        /// </summary>
        public string PropertyComment { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        public Int64 MaxLength { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool? IsRequire { get; set; }


        /// <summary>
        /// 表名
        /// </summary>
        public string Table { get; set; }
    }
}
