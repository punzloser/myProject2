using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.ProductImages;
using ViewModel.Catalog.Products;

namespace Application.Catalog.Products
{
    public interface IProductService
    {
        //input request. return kieu int -- ma sp vua tao
        Task<int> Create(ProductCreateRequest request);
        Task<int> Edit(int productId, ProductEditRequest request);
        Task<int> Delete(int id);
        Task<ProductViewModel> GetById(int productId, string languageId);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addQuantity);
        Task AddViewCount(int productId);
        Task<int> AddImg(ProductImageCreate create, int productId);
        Task<int> EditImg(ProductImageEdit edit, int ImgId);
        Task<int> DeleteImg(int ImgId);
        Task<List<ProductViewModel>> GetLaptopLatest(string languageId, int quantity);
        Task<List<ProductViewModel>> GetMobileLatest(string languageId, int quantity);

        Task<List<ProductImageViewModel>> GetListImgById(int productId);
        Task<ProductImageViewModel> GetImgById(int imgId);

        Task<PageResult<ProductViewModel>> GetAllPaging(AdminProductPaging request);
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(PublicProductPaging request, string languageId);
    }
}
