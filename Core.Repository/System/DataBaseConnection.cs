using FreeSql;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository
{
    public class DataBaseConnection
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; } 
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; set; }


        /// <summary>
        /// 连接字符串
        /// </summary>
        [Column(StringLength = 2000)]
        public string ConnectinString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DataBaseType { get; set; }
    }
}
