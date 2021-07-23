using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Catalog.Roles
{
    public class RoleEditModel
    {
        public Guid Id { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
    public class SelectItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
