using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace ViewModel.Catalog.Products
{
    public class PublicProductPaging : PageRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
