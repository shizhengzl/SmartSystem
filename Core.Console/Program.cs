using Core.Repository.DataBase;
using Core.Repository.Generator;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.UsuallyCommon;

namespace Core.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IFreeSql systemsql = new FreeSqlBuilder()
                 .UseConnectionString(DataType.SqlServer, "server=47.104.62.7;uid=sa;pwd=Tcn6500;database=DefaultSqlite")
                 .UseAutoSyncStructure(false)
                 .Build();

            systemsql.CodeFirst.SyncStructure<Column>();
            systemsql.CodeFirst.SyncStructure<DBService>();

            #region 初始化连接数据
            var dbservicecount = systemsql.Select<DBService>().Count();
            if (dbservicecount == 0)
            {
                DBService dBSqlserver = new DBService() { Account = "sa", DataType = DataType.SqlServer, Password = "Tcn6500", ServerAddress = "47.104.62.7", DefaultDataBase = "SmartsApplicationDb" };
                //DBService dBMysql = new DBService() { Account = "sa", DataType = DataType.MySql, Password = "Tcn6500", ServerAddress = "47.104.62.7", DefaultDataBase = "SmartsApplicationDb" };
                systemsql.Insert<DBService>().AppendData(dBSqlserver).ExecuteAffrows();
                //systemsql.Insert<DBService>().AppendData(dBMysql).ExecuteAffrows();
            }
            #endregion

            List<IFreeSql> freeSqls = new List<IFreeSql>();
            List<DataBaseTree> dataBaseTrees = new List<DataBaseTree>();
            SQLConfigHelper _sqlconfig = new SQLConfigHelper(systemsql);
            var dbservices = systemsql.Select<DBService>();

            dbservices.ToList<DBService>().ForEach(x =>
            {
                var tablesql = _sqlconfig.GetTables(x.DataType);
                var columnsql = _sqlconfig.GetColumns(x.DataType);

                if (x.DataType == DataType.SqlServer)
                {
                    var connectionstring = $"server={x.ServerAddress};uid={x.Account};pwd={x.Password};database={x.DefaultDataBase}";
                    ADOHelper.connectionString = connectionstring; 

                    IFreeSql generatorfreesql = new FreeSqlBuilder()
                     .UseConnectionString(x.DataType, connectionstring)
                     .UseAutoSyncStructure(false)
                     .Build();

                    List<Table> tables = ADOHelper.ExecuteQuery(tablesql).Tables[0].ToList<Table>(); 
                    tables.ForEach(p =>
                    {
                        p.DataBaseName = x.DefaultDataBase;
                        p.ServerAddress = x.ServerAddress;  
                    });

                    var databasetree = new DataBaseTree()
                    {
                        Account = x.Account,
                        DataType = x.DataType,
                        DefaultDataBase = x.DefaultDataBase,
                        Id = x.Id,
                        Password = x.Password,
                        ServerAddress = x.ServerAddress,
                        Tables = tables,
                        GetColumnString = columnsql,
                        ConnectionString = connectionstring
                    }; 
                    dataBaseTrees.Add(databasetree);
                     
                    var sql = string.Format(databasetree.GetColumnString, x.ServerAddress, x.DefaultDataBase, tables.First().TableName);  
                    var columns = ADOHelper.ExecuteQuery(sql).Tables[0].ToList<Column>();
                     
                    freeSqls.Add(generatorfreesql);

                }
            });

        }
    }

    
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
