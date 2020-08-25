using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Repository;
using Core.UsuallyCommon;
using System.Linq;

namespace Core.Windows.ControlTools
{
    public class ToolScriptExtension<T> : ToolStrip where T : class, new()
    { 
        public PanelExtension<T> Panel { get; set; }
        public ToolScriptExtension(PanelExtension<T> panel)
        {
            Panel = panel;

            this.Dock = DockStyle.Fill;
            List<string> btn = Core.UsuallyCommon.EnumExtensions.EnumToList<ToolScriptButton>();
            foreach (var item in btn)
            {
                // add button 
                ToolStripButton button = new ToolStripButton() { Text = item, Name = $"btn{item}" };
                button.Click += Button_Click;

                var btnenum = item.ToEnum<ToolScriptButton>();

                //switch (btnenum)
                //{
                //    case ToolScriptButton.Insert:
                //        button.Image = tools.imageList.Images[(int)ImageEnum.Add];
                //        break;
                //    case ToolScriptButton.Update:
                //        button.Image = tools.imageList.Images[(int)ImageEnum.Edit];
                //        break;
                //    case ToolScriptButton.Delete:
                //        button.Image = tools.imageList.Images[(int)ImageEnum.Remove];
                //        break;
                //    case ToolScriptButton.Refresh:
                //        button.Image = tools.imageList.Images[(int)ImageEnum.Refresh];
                //        break;
                //}
                this.Items.Add(button);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton)sender;
            var btnenum = button.Text.ToEnum<ToolScriptButton>();
            switch (btnenum)
            {
                case ToolScriptButton.Insert:
                    WindowExtension<T> windowinsert = new WindowExtension<T>(new T(), true);
                    DialogResult dialoginsert = windowinsert.ShowDialog();
                    break;
                case ToolScriptButton.Update:
                    var rows = Panel._gridView.SelectedRows;
                    if (rows.Count == 0)
                    {
                        return;
                    }
                    var result = FreeSqlFactory._Freesql.Select<T>().Skip(Panel._gridView.SelectedRows[0].Index).Take(1).First(); //ExtenstionClass.GetList<T>(new DefaultSqlite()).Skip(Panel.gridView.SelectedRows[0].Index).Take(1).FirstOrDefault();
                    WindowExtension<T> windowupdate = new WindowExtension<T>(result, false);
                    DialogResult dialogupdate = windowupdate.ShowDialog();
                    break;
                case ToolScriptButton.Delete: 
                    var resultDelete = FreeSqlFactory._Freesql.Select<T>().Skip(Panel._gridView.SelectedRows[0].Index).Take(1).First();
                    FreeSqlFactory._Freesql.Delete<T>(resultDelete).ExecuteAffrows();
                    break;
                case ToolScriptButton.Refresh:
                    break;
            }
            Panel._gridView.DataSource = FreeSqlFactory._Freesql.Select<T>().ToList();
        }


    }
}