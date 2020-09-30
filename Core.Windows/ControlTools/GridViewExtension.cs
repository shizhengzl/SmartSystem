using Core.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Windows.ControlTools
{
    public class GridViewExtension<T> : DataGridView where T : class, new()
    {

        public GridViewExtension()
        {
            this.Dock = DockStyle.Fill;
            this.DataSource = FreeSqlFactory._Freesql.Select<T>().ToList();
            this.AutoGenerateColumns = true;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.BackgroundColor = Color.White;
            this.CellDoubleClick += GridViewExtension_CellDoubleClick;
            this.RowsDefaultCellStyle.BackColor = Color.Violet;
            this.AlternatingRowsDefaultCellStyle.BackColor = Color.Yellow;
        }

        private void GridViewExtension_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var objects = (T)this.CurrentRow.DataBoundItem;//visit  相当于一个实体

            WindowExtension<T> window = new WindowExtension<T>(objects, false);

            DialogResult dialog = window.ShowDialog();

            this.DataSource = FreeSqlFactory._Freesql.Select<T>().ToList();
        }
    }
}