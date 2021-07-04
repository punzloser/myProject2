using Application.Catalog.Product.DTO;
using Application.Catalog.Product.DTO.Manage;
using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Product
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

        Task<List<ProductViewModel>> GetAll();
        Task<PageResult<ProductViewModel>> GetAllPaging(ProductPagingRequest request);
    }
}
