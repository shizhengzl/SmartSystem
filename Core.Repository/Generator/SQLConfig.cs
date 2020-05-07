using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository.Generator
{
    /// <summary>
    /// SQL配置类
    /// </summary>
    public class SQLConfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsPrimary = true, IsIdentity =true)]
        public Int32 Id { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType Type { get; set; }
        /// <summary>
        /// 查询所有数据库SQL
        /// </summary>
        public string GetDataBaseSQL { get; set; }
        /// <summary>
        /// 查询数据库所有表SQL
        /// </summary>
        public string GetTableSQL { get; set; }
        /// <summary>
        /// 获取表所有列SQL
        /// </summary>
        public string GetColumnSQL { get; set; }
        /// <summary>
        /// 获取所有存储过程SQL
        /// </summary>
        public string GetProducuteSQL { get; set; }
        /// <summary>
        /// 获取所有视图SQL
        /// </summary>
        public string GetViewSQL { get; set; }
        /// <summary>
        /// 获取所有索引SQL
        /// </summary>
        public string GetIndexSQL { get; set; }
        /// <summary>
        /// 获取所有同义词SQL
        /// </summary>
        public string GetSYNONYMSQL { get; set; }
    }
}
