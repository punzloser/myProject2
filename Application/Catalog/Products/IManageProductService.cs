using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.ProductImages;
using ViewModel.Catalog.Products;

namespace Application.Catalog.Products
{
    public interface IManageProductService
    {
        //input request. return kieu int -- ma sp vua tao
        Task<int> Create(ProductCreateRequest request);
        Task<int> Edit(ProductEditRequest request);
        Task<int> Delete(int id);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addQuantity);
        Task AddViewCount(int productId);
        Task<int> AddImg(ProductImageCreate create, int ProductId);
        Task<List<ProductImageViewModel>> GetAllImg(int ProductId);

        Task<PageResult<ProductViewModel>> GetAllPaging(AdminProductPaging request);
    }
}
