using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.ProductSlides;

namespace ApiClientCommon.Service
{
    public interface IProductSlideApiClient
    {
        Task<List<ProductSlideVm>> GetAllByProductId(int ProductId);
    }
}
