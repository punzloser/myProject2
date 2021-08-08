using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Categories;
using ViewModel.Catalog.Products;
using ViewModel.Catalog.ProductSlides;

namespace WebApp.Models
{
    public class ProductDetail
    {
        public List<ProductSlideVm> ProductSlides { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
