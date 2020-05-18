using Core.Repository.DataBase;
using Core.Repository.Generator;
using FreeSql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.UsuallyCommon;

namespace Core.Windows
{
    public partial class Generators : Form
    {
       public static IFreeSql systemsql = new FreeSqlBuilder()
            .UseConnectionString(DataType.SqlServer, "server=47.104.62.7;uid=sa;pwd=Tcn6500;database=DefaultSqlite")
            .UseAutoSyncStructure(false)
            .Build();

        List<IFreeSql> freeSqls = new List<IFreeSql>();
        List<DataBaseTree> dataBaseTrees = new List<DataBaseTree>();
        SQLConfigHelper _sqlconfig = new SQLConfigHelper(systemsql);

        public Generators()
        {
            InitializeComponent();

            TreeNode root = new TreeNode() { 
                Text = "服务器"
            };

            treeViewDatabase.Nodes.Add(root);

            var addresses = systemsql.Select<DBService>().ToList();


            foreach (var address in addresses)
            {
                var addressNode = new TreeNode() { 
                    Text = address.ServerAddress
                };
                root.Nodes.Add(addressNode);

                // 获取Table
                var tablesql = _sqlconfig.GetTables(address.DataType);
                var columnsql = _sqlconfig.GetColumns(address.DataType);

                if (address.DataType == DataType.SqlServer)
                {
                    var connectionstring = $"server={address.ServerAddress};uid={address.Account};pwd={address.Password};database={address.DefaultDataBase}";
                    ADOHelper.connectionString = connectionstring;

                    IFreeSql generatorfreesql = new FreeSqlBuilder()
                     .UseConnectionString(address.DataType, connectionstring)
                     .UseAutoSyncStructure(false)
                     .Build();

                    List<Table> tables = ADOHelper.ExecuteQuery(tablesql).Tables[0].ToList<Table>();
                    tables.ForEach(p =>
                    {
                        p.DataBaseName = address.DefaultDataBase;
                        p.ServerAddress = address.ServerAddress;
                    });

                    var databasetree = new DataBaseTree()
                    {
                        Account = address.Account,
                        DataType = address.DataType,
                        DefaultDataBase = address.DefaultDataBase,
                        Id = address.Id,
                        Password = address.Password,
                        ServerAddress = address.ServerAddress,
                        Tables = tables,
                        GetColumnString = columnsql,
                        ConnectionString = connectionstring
                    };
                    dataBaseTrees.Add(databasetree);

                    var sql = string.Format(databasetree.GetColumnString, address.ServerAddress, address.DefaultDataBase, tables.First().TableName);
                    var columns = ADOHelper.ExecuteQuery(sql).Tables[0].ToList<Column>();

                    freeSqls.Add(generatorfreesql);

                }
            }
        }
    }
}
