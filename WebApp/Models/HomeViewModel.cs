using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Products;

namespace WebApp.Models
{
    //tập hợp để trả về nhiều thứ nên phải tạo model riêng để tổng hợp từ ViewModel 
    public class HomeViewModel
    {
        public List<CarouselViewModel> Carousels { get; set; }
        public List<ProductViewModel> LaptopLatest { get; set; }
        public List<ProductViewModel>  MobileLatest{ get; set; }
    }
}
