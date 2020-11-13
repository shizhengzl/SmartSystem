using Core.Repository;
using Core.Repository.Generator;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Text;
using Core.UsuallyCommon;

namespace Core.Services
{
    public class SQLConfigServices
    {

        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetDataBases(DataType dataType)
        {
            return FreeSqlFactory._Freesql.Select<SQLConfig>().Where(x => x.Type == dataType).First().GetDataBaseSQL.ToStringExtension();
        }

        /// <summary>
        /// 获取数据库的表
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetTables(DataType dataType)
        {
            return FreeSqlFactory._Freesql.Select<SQLConfig>().Where(x => x.Type == dataType).First().GetTableSQL.ToStringExtension();
        }

        /// <summary>
        /// 获取数据库表的列
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string GetColumns(DataType dataType)
        {
            return FreeSqlFactory._Freesql.Select<SQLConfig>().Where(x => x.Type == dataType).First().GetColumnSQL.ToStringExtension();
        }


        /// <summary>
        /// 获取数据库表的列
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string AddExtendedproperty(DataType dataType)
        {
            return FreeSqlFactory._Freesql.Select<SQLConfig>().Where(x => x.Type == dataType).First().AddExtendedproperty.ToStringExtension();
        }


        /// <summary>
        /// 获取数据库表的列
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string ModifyExtendedproperty(DataType dataType)
        {
            return FreeSqlFactory._Freesql.Select<SQLConfig>().Where(x => x.Type == dataType).First().ModifyExtendedproperty.ToStringExtension();
        }



        /// <summary>
        /// 获取数据库表的列
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string AddTableExtendedproperty(DataType dataType)
        {
            return FreeSqlFactory._Freesql.Select<SQLConfig>().Where(x => x.Type == dataType).First().AddTableExtendedproperty.ToStringExtension();
        }


        /// <summary>
        /// 获取数据库表的列
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string ModifyTableExtendedproperty(DataType dataType)
        {
            return FreeSqlFactory._Freesql.Select<SQLConfig>().Where(x => x.Type == dataType).First().ModifyTableExtendedproperty.ToStringExtension();
        }
    }
}
