using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Products;

namespace Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(PublicProductPaging request);
    }
}
