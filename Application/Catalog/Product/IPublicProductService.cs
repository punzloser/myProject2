using Application.Catalog.Product.DTO;
using Application.Catalog.Product.DTO.Public;
using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Product
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(ProductPagingRequest request);
    }
}
