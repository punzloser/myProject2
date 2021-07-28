using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace ViewModel.Catalog.Roles
{
    public class RoleEditModel
    {
        public Guid Id { get; set; }
        public List<Item> Roles { get; set; } = new List<Item>();
    }
    
}
