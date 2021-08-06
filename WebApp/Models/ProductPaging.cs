using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Categories;
using ViewModel.Catalog.Products;

namespace WebApp.Models
{
    public class ProductPaging
    {
        public CategoryViewModel Category { get; set; }
        public PageResult<ProductViewModel> Products { get; set; }
    }
}
