using FreeSql;
using System;
using System.Collections.Generic;
using System.Text;
using Core.UsuallyCommon;

namespace Core.Repository.Generator
{
    /// <summary>
    /// 读取配置数据
    /// </summary>
    public class SQLConfigHelper
    {
        /// <summary>
        /// freesql
        /// </summary>
        public IFreeSql _freeSql { get; set; }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="freeSql"></param>
        public SQLConfigHelper(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        } 

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetDataBases(DataType dataType)
        {
            return _freeSql.Select<SQLConfig>().Where(x => x.Type == dataType).First().GetDataBaseSQL.ToStringExtension();
        }  

        /// <summary>
        /// 获取数据库的表
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetTables(DataType dataType)
        {
            return _freeSql.Select<SQLConfig>().Where(x => x.Type == dataType).First().GetTableSQL.ToStringExtension();
        }

        /// <summary>
        /// 获取数据库表的列
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetColumns(DataType dataType)
        {
            return _freeSql.Select<SQLConfig>().Where(x => x.Type == dataType).First().GetColumnSQL.ToStringExtension();
        }
    }
}
