using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.ProductSlides;

namespace Application.Catalog.ProductSlides
{
    public interface IProductSlideService
    {
        Task<List<ProductSlideVm>> GetAllByProductId(int ProductId);
    }
}
