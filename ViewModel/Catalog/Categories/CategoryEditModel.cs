using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace ViewModel.Catalog.Categories
{
    public class CategoryEditModel
    {
        public int Id { get; set; }
        public List<Item> Categories { get; set; } = new List<Item>();
    }
}
