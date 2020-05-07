using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository.Generator
{
    /// <summary>
    /// 表
    /// </summary>
    public class Table
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
    }
}
