using System;
using System.Collections.Generic;
using System.Text;
using Core.UsuallyCommon;
using FreeSql;

namespace Core.Repository
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class FreeSqlFactory
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DefaultBaseConnection
        {
            get
            {
                //var connectionstring = "Data Source=cd-cdb-5dczq90q.sql.tencentcdb.com;port=62299;Initial Catalog=DefaultSqlite;uid=root;password=Shizi120;Charset=utf8";
                //var connectionstring = "Data Source=192.168.0.99;Initial Catalog=JZB_TEST;Persist Security Info=True;User ID=empsadmin;Password=BatSe686mpsadmin((sdf#%=iosxXYy;pooling=false;";
                var connectionstring = "Data Source=.;Initial Catalog=SmartDb;Persist Security Info=True;User ID=sa;Password=sasa;";
                var dc =System.Configuration.ConfigurationManager.ConnectionStrings["DefaultBaseConnection"].ToStringExtension();
                if (!string.IsNullOrEmpty(dc))
                    connectionstring = dc;
                return connectionstring;
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DataType GetDataType
        {
            get
            {
                //var datetype = DataType.MySql;
                var datetype = DataType.SqlServer;
                var datatypestring = System.Configuration.ConfigurationManager.ConnectionStrings["DataType"].ToStringExtension();
                if (!string.IsNullOrEmpty(datatypestring))
                    datetype = System.Configuration.ConfigurationManager.ConnectionStrings["DataType"].ToStringExtension().ToEnum<DataType>();
                return datetype; 
            }
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        public static String ReplaceString
        {
            get
            {
                var replacestring = "@TableName";
                var rp = System.Configuration.ConfigurationManager.ConnectionStrings["ReplaceString"].ToStringExtension();
                if (!string.IsNullOrEmpty(rp))
                    replacestring = rp;
                return replacestring;
            }
        }


        /// <summary>
        /// 获取连接池
        /// </summary>
        /// <param name="DbType"></param>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static IFreeSql GetFreeSql(DataType DbType, String Connection)
        {
            //DataType dataType = DbType.ToStringExtension().ToEnum<DataType>();
            return new FreeSqlBuilder()
             .UseConnectionString(DbType, Connection)
             .UseAutoSyncStructure(false)
             .Build();
        }


        /// <summary>
        /// 获取连接池
        /// </summary>
        /// <param name="DbType"></param>
        /// <param name="Connection"></param>
        /// <returns></returns>
        public static IFreeSql GetFreeSql()
        {
            return _Freesql;
        }

        public static IFreeSql _Freesql = new FreeSqlBuilder()
             .UseConnectionString(GetDataType, DefaultBaseConnection)
             .UseAutoSyncStructure(true)
             .Build();
    }
}
