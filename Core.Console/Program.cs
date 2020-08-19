using Core.Repository.DataBase;
using Core.Repository.Generator;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.UsuallyCommon;
using Core.Services;

namespace Core.Console
{

  
    class Program
    {

        public static void MadeDateTime(DateTime standTime, out DateTime? startTime, out DateTime? endTime)
        {
            //1~3;4~6;7~9;10~12
            //1-4-7-10
            int baseMonth = standTime.Month - 1;
            int MonthLen = baseMonth / 3;
            startTime = new DateTime(standTime.Year, 1 + MonthLen * 3, 1);
            endTime = startTime.Value.AddMonths(3).AddMilliseconds(-1);
        }

         
        static void Main(string[] args)
        {
            DateTime ? startDate = DateTime.MinValue;
            DateTime ? endDate = DateTime.MinValue; 
            MadeDateTime("2020-10-10".ToDateTime(), out startDate , out endDate);

            System.Console.WriteLine(startDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            System.Console.WriteLine(endDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            System.Console.ReadLine();
            return;

            var connection = @"server=DESKTOP-5GDQD44\MSSQL;uid=sa;pwd=sasa;database=Tools";

            IFreeSql freesqls = new FreeSqlBuilder()
              .UseConnectionString(DataType.SqlServer, connection)
              .UseAutoSyncStructure(false)
              .Build();

            freesqls.CodeFirst.SyncStructure<Column>();



            List<String> listUrl = new List<string>();
            List<Column> columns = new List<Column>();
            Core.UsuallyCommon.FileExtenstion.GetFileByExtension(@"D:\Test", "*.cs", ref listUrl);

            //var parh = @"D:\workcode\emps\trunk\Entity\bin\Debug";
            //var emus = CsharpParser.GetEnums(parh);
            //return;
            listUrl.ForEach(x =>
            {
                CsharpParser parser = new CsharpParser(x);
                var classes = parser.GetClasses();
                classes.ForEach(u =>
                { 
                    var className = u.Identifier.Text;  
                    var classcomment = CsharpParser.GetClassComment(u);

                    var allproperty = parser.GetCsharpClassProperty(u);
                    allproperty.ForEach(o =>
                    {
                        Column column = new Column()
                        {
                            ColumnName = o.PropertyName,
                            ColumnDescription = o.PropertyComment,
                            CSharpType = o.PropertyType.Replace("?", string.Empty),
                            IsRequire = !(o.PropertyType.IndexOf("?") > -1),
                            MaxLength = o.MaxLength,
                            TableName = o.Table,
                            TableDescription = classcomment
                        };

                        columns.Add(column);

                        //var execute = freesqls.Insert<Column>(column).ExecuteAffrows();
                         
                        //if (!dbContext.DefaultColumns.Any(z => z.ColumnName == column.ColumnName && z.CSharpType == column.CSharpType && z.Table == column.Table))
                        //{
                        //    dbContext.DefaultColumns.Add(column);
                        //    dbContext.SaveChanges();
                        //} 
                    }); 
                });
            });
            var rs = columns.Where(o => o.TableName == "Project");



            return;
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
                DBService dBSqlserver = new DBService() { 
                    Account = "sa"
                    , DataType = DataType.SqlServer
                    , Password = "Tcn6500"
                    , ServerAddress = "47.104.62.7"
                    , DefaultDataBase = "SmartsApplicationDb" 
                };
                
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
                    var lcolumns = ADOHelper.ExecuteQuery(sql).Tables[0].ToList<Column>();
                     
                    freeSqls.Add(generatorfreesql);

                }
            });

        }
    }

    
 
}
