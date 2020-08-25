using Core.UsuallyCommon;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Intellisence
{
    ///// <summary>
    ///// 数据库操作类
    ///// </summary>
    //public static class RepositoryFactory
    //{
    //    /// <summary>
    //    /// 数据库连接字符串
    //    /// </summary>
    //    public static string DefaultBaseConnection {
    //        get
    //        {
    //            return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultBaseConnection"].ToStringExtension();
    //        }
    //    }

    //    /// <summary>
    //    /// 数据库类型
    //    /// </summary>
    //    public static DataType GetDataType {
    //        get
    //        {
    //            return System.Configuration.ConfigurationManager.ConnectionStrings["DataType"].ToStringExtension().ToEnum<DataType>();
    //        }
    //    }

    //    /// <summary>
    //    /// 替换字符串
    //    /// </summary>
    //    public static String ReplaceString
    //    {
    //        get
    //        {
    //            return System.Configuration.ConfigurationManager.ConnectionStrings["ReplaceString"].ToStringExtension();
    //        }
    //    }

    //    /// <summary>
    //    /// 获取连接池
    //    /// </summary>
    //    /// <param name="DbType"></param>
    //    /// <param name="Connection"></param>
    //    /// <returns></returns>
    //    public static IFreeSql GetFreeSql(Int16 DbType,String Connection)
    //    {
    //        DataType dataType = DbType.ToStringExtension().ToEnum<DataType>();
    //        return new FreeSqlBuilder()
    //         .UseConnectionString(dataType, Connection)
    //         .UseAutoSyncStructure(false)
    //         .Build();
    //    }

    //    public static IFreeSql _Freesql = new FreeSqlBuilder()
    //         .UseConnectionString(GetDataType, DefaultBaseConnection)
    //         .UseAutoSyncStructure(true)
    //         .Build();

    //}




}
