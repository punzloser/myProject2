using Application.Catalog.Product.DTO;
using Application.Catalog.Product.DTO.Public;
using Application.DTO;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Product
{
    public class PublicProductService : IPublicProductService
    {
        private readonly MyDBContext _db;
        public PublicProductService(MyDBContext db)
        {
            _db = db;
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(ProductPagingRequest request)
        {
            //1 join
            var result = from a in _db.Products
                         join b in _db.ProductTranslations on a.Id equals b.ProductId
                         join c in _db.ProductCategories on a.Id equals c.ProductID
                         join d in _db.Categories on c.CategoryID equals d.Id
                         select new
                         {
                             a,
                             b,
                             c
                         };
            //2 filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
                result = result.Where(p => p.c.CategoryID == request.CategoryId);
            //3 paging
            int totalRow = await result.CountAsync();
            var data = await result
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.a.Id,
                    Name = x.b.Name,
                    DateCreated = x.a.DateCreated,
                    Description = x.b.Description,
                    Details = x.b.Details,
                    LanguageId = x.b.LanguageId,
                    Price = x.a.Price,
                    OriginalPrice = x.a.OriginalPrice,
                    SeoAlias = x.b.SeoAlias,
                    SeoTitle = x.b.SeoTitle,
                    SeoDescription = x.b.SeoDescription,
                    Stock = x.a.Stock,
                    ViewCount = x.a.ViewCount
                }).ToListAsync();

            var pageResult = new PageResult<ProductViewModel>()
            {
                ToTalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }
    }
}
