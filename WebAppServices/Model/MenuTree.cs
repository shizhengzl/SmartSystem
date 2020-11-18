using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServices.Model
{
    public class MenuTree
    {
        public Int32 Id { get; set; }
        public string path { get; set; }

        public string component { get; set; }

        public string redirect { get; set; }

        public string name { get; set; }

        public bool alwaysShow { get; set; }

        public MenuMeta meta { get; set; }

        public List<MenuTree> children { get; set; }
    }
}
