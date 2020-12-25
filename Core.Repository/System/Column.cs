using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    /// <summary>
    /// 列
    /// </summary>
    public class Column
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        [Description("主键")]
        public Int64 Id { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
       [Description("服务地址")]
        public string ServerAddress { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        [Description("数据库名称")]
        public string DataBaseName { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        [Description("表名")]
        public string TableName { get; set; }
        /// <summary>
        /// 表描述
        /// </summary>
        [Description("表描述")]
        public string TableDescription { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string ColumnName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        public string ColumnDescription { get; set; }
        /// <summary>
        /// CsharpType
        /// </summary>
        [Description("CsharpType")]
        public string CSharpType { get; set; }
        /// <summary>
        /// 是否自增
        /// </summary>
        [Description("是否自增")]
        public bool IsIdentity { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        [Description("是否主键")]
        public bool IsPrimarykey { get; set; }
        /// <summary>
        /// 最大长度
        /// </summary>
        [Description("最大长度")]
        public Int64? MaxLength { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        [Description("是否必填")]
        public bool IsRequire { get; set; }
        /// <summary>
        /// 精度
        /// </summary>
        [Description("精度")]
        public byte Scale { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [Description("默认值")]
        public string DefaultValue { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        [Description("数据库类型")]
        public string SQLType { get; set; }

    }
}
