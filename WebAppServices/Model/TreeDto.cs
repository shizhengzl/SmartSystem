using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServices.Model
{
    public class TreeDto
    {
        public Int64 id { get; set; } 
        public string label { get; set; } 
        public List<TreeDto> children { get; set; }


        public Int64 parentId { get; set; }

        public string description { get; set; }
    }
}
