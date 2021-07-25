using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Products;

namespace Manager.Service
{
    public interface IProductApiClient
    {
        Task<PageResult<ProductViewModel>> GetProductPaging(AdminProductPaging paging);
    }
}
