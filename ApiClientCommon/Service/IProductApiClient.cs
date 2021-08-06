using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Products;

namespace ApiClientCommon.Service
{
    public interface IProductApiClient
    {
        Task<PageResult<ProductViewModel>> GetProductPaging(AdminProductPaging paging);
        Task<PageResult<ProductViewModel>> GetProductPagingByCategoryId(AdminProductPaging paging, int categoryId);
        Task<bool> Create(ProductCreateRequest request);
        Task<bool> Edit(ProductEditRequest request);
        Task<ProductViewModel> GetById(int productId, string languageId);
        Task<List<ProductViewModel>> GetLaptopLatest(string languageId, int quantity);
        Task<List<ProductViewModel>> GetMobileLatest(string languageId, int quantity);
    }
}
