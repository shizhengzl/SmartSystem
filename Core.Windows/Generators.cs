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
using Core.Repository;
using Core.Windows.ControlTools;

namespace Core.Windows
{
    public partial class Generators : Form
    { 
        List<IFreeSql> freeSqls = new List<IFreeSql>();
        List<DataBaseTree> dataBaseTrees = new List<DataBaseTree>();
        SQLConfigHelper _sqlconfig = new SQLConfigHelper();


        #region SystemConfig

        public void InitSystemConfig()
        {
          
            InitClass<SQLConfig>();
            
            InitClass<DataTypeConfig>();
          
            InitClass<Intellisence>();

            InitClass<SystemLogs>(); 


        }

        public void InitClass<T>() where T : class, new()
        {
            var type = typeof(T);
            var className = type.Name;
            TabPage tpclass = new TabPage() { Name = className, Text = className  }; 
            PanelExtension<T> panel = new PanelExtension<T>();
            tpclass.Controls.Add(panel);
            tabControls.TabPages.Add(tpclass);
        }
        #endregion 

        public Generators()
        {
            InitializeComponent();
            this.skinEngines.SkinFile = $"{System.Environment.CurrentDirectory}\\Plugins\\Skins\\SportsCyan.ssk";
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            InitSystemConfig();

            // 初始化数据
            InitDatabase initDatabase = new InitDatabase(true);

            TreeNode root = new TreeNode() { 
                Text = "服务器"
            };

            treeViewDatabase.Nodes.Add(root);

            var addresses = FreeSqlFactory._Freesql.Select<DBService>().ToList();


            foreach (var address in addresses)
            {
                var addressNode = new TreeNode() { 
                    Text = address.ServerAddress
                };
                root.Nodes.Add(addressNode);

                // 获取Table
                var tablesql = string.Format(_sqlconfig.GetTables(address.DataType),address.ServerAddress,address.DefaultDataBase);
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

                        TreeNode tableNode = new TreeNode()
                        {
                            Text = p.TableName,
                            ToolTipText = p.TableDescription,
                            Tag = p
                        };

                        tableNode.Nodes.Add(string.Empty);

                        addressNode.Nodes.Add(tableNode);
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

                    //var sql = string.Format(databasetree.GetColumnString, address.ServerAddress, address.DefaultDataBase, tables.First().TableName);
                    //var columns = ADOHelper.ExecuteQuery(sql).Tables[0].ToList<Column>();

                    freeSqls.Add(generatorfreesql);

                }
            }
        }
    }
}
