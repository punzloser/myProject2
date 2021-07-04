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

        public async Task<int> Edit(ProductEditRequest request)
        {
            var product = await _db.Products.FindAsync(request.Id);
            var productTranslation = await _db.ProductTranslations
                .FirstOrDefaultAsync(a => a.Id == request.Id && a.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null)
            {
                throw new CallException($"Không tìm thấy sản phẩm {request.Id}");
            }

            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.Details = request.Details;
            productTranslation.Description = request.Description;
            productTranslation.SeoTitle = request.SeoTitle;

            return await _db.SaveChangesAsync();
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

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null)
            {
                throw new CallException($"Không tìm thấy sản phẩm {productId}");
            }

            product.Price = newPrice;

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addQuantity)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product == null)
            {
                throw new CallException($"Không tìm thấy sản phẩm {productId}");
            }

            product.Stock += addQuantity;

            return await _db.SaveChangesAsync() > 0;
        }
    }
}
