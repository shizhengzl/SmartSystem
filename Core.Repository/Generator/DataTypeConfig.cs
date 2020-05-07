using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository.Generator
{
    /// <summary>
    /// 数据类型映射关系
    /// </summary>
    public class DataTypeConfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        public Int32 Id { get; set; }
        /// <summary>
        /// SQL类型
        /// </summary>
        public string SQLServerType { get; set; }
        /// <summary>
        /// Mysql类型
        /// </summary>
        public string MySqlType { get; set; }
        /// <summary>
        /// Oracle类型
        /// </summary>
        public string OracleType { get; set; }
        /// <summary>
        /// Sqlite类型
        /// </summary>
        public string SQLiteType { get; set; }
        /// <summary>
        /// C#类型
        /// </summary>
        public string CSharpType { get; set; }
        /// <summary>
        /// 数据库DB类型
        /// </summary>
        public string SQLDBType { get; set; }
    }
}
