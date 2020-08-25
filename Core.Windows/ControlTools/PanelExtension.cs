using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Windows.ControlTools
{
    public class PanelExtension<T> : Panel where T : class, new()
    {
        public GridViewExtension<T> _gridView { get; set; }

        public ToolScriptExtension<T> _toolScript { get; set; }
        public PanelExtension()
        {
            this.Dock = DockStyle.Fill;
            _toolScript = new ToolScriptExtension<T>(this);
            _gridView = new GridViewExtension<T>();

            Panel pt = new Panel() { Height = 40 };
            Panel ptg = new Panel();
            pt.Controls.Add(_toolScript);
            ptg.Controls.Add(_gridView);

            this.Controls.Add(ptg);
            this.Controls.Add(pt);
            pt.Dock = DockStyle.Top;
            ptg.Dock = DockStyle.Fill;

        }
    }
}
