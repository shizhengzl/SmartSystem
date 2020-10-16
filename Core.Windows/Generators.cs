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
using Core.Services;

namespace Core.Windows
{
    public partial class Generators : Form
    { 
        List<IFreeSql> freeSqls = new List<IFreeSql>(); 

        #region SystemConfig

        public void InitSystemConfig()
        {
          
            InitClass<SQLConfig>();
            
            InitClass<DataTypeConfig>();
          
            InitClass<Intellisence>();

            InitClass<SystemLogs>();

            InitClass<DataBaseConnection>();
            
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
            this.skinEngines.SkinFile = $"{System.Environment.CurrentDirectory}\\Plugins\\Skins\\OneOrange.ssk";
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            InitSystemConfig();
            DataBaseServices services = new DataBaseServices();
            // 初始化数据
            InitDatabase initDatabase = new InitDatabase(true);

            TreeNode root = new TreeNode() { 
                Text = "服务器"
            };

            treeViewDatabase.Nodes.Add(root);

              
        }
    }
}
