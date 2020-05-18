using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository.Generator
{
    /// <summary>
    /// DBTREE
    /// </summary>
    public class DataBaseTree : DBService
    {
        /// <summary>
        /// 获取列SQL
        /// </summary>
        public string GetColumnString { get; set; }

        /// <summary>
        /// 表
        /// </summary>
        public List<Table> Tables { get; set; }


        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
