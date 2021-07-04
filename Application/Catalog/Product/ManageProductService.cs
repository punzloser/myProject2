using Application.Catalog.Product.DTO;
using Application.Catalog.Product.DTO.Manage;
using Application.DTO;
using Common.Exceptions;
using Data;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Product
{
    public class ManageProductService : IManageProductService
    {
        private readonly MyDBContext _db;
        public ManageProductService(MyDBContext db)
        {
            _db = db;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Data.Entity.Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        SeoAlias = request.SeoAlias,
                        LanguageId  = request.LanguageId
                    }
                }
            };
            _db.Products.Add(product);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) throw new CallException($"Không tìm thấy sản phẩm {id}");

            _db.Products.Remove(product);
            return await _db.SaveChangesAsync();
        }

        public Task<int> Edit(ProductEditRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(ProductPagingRequest request)
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
            if (!string.IsNullOrEmpty(request.Keyword))
                result = result.Where(p => p.b.Name.Contains(request.Keyword));

            if (request.CategoryIds.Any())
                result = result.Where(p => request.CategoryIds.Contains(p.c.CategoryID));
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

        public async Task AddViewCount(int productId)
        {
            var product = await _db.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _db.SaveChangesAsync();
        }

        public Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int productId, int addQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
