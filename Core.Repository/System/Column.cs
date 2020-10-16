using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
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
        public Int64 Id { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServerAddress { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表描述
        /// </summary>
        public string TableDescription { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string ColumnDescription { get; set; }
        /// <summary>
        /// CsharpType
        /// </summary>
        public string CSharpType { get; set; }
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimarykey { get; set; }
        /// <summary>
        /// 最大长度
        /// </summary>
        public Int64? MaxLength { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequire { get; set; }
        /// <summary>
        /// 精度
        /// </summary>
        public byte Scale { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string SQLType { get; set; }

    }
}
