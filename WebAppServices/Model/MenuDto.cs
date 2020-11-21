using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServices.Model
{
    public class MenuDto : Menus
    {
        public List<MenuDto> children { get; set; }
    }
}
