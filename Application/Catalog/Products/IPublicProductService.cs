using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Products;

namespace Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(PublicProductPaging request);
        Task<List<ProductViewModel>> GetAll();
    }
}
