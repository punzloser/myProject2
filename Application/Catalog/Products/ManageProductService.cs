using Application.Common;
using Common.Exceptions;
using Data;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.ProductImages;
using ViewModel.Catalog.Products;

namespace Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly MyDBContext _db;
        private readonly IStorageService _storage;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        public ManageProductService(MyDBContext db, IStorageService storage)
        {
            _db = db;
            _storage = storage;
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

            //save img
            if (request.Thumnail != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumnail-img",
                        DateCreated = DateTime.Now,
                        FileSize = request.Thumnail.Length,
                        ImagePath = await this.SaveImg(request.Thumnail),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }

            _db.Products.Add(product);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) throw new CallException($"Không tìm thấy sản phẩm {id}");

            var img = _db.ProductImages.Where(a => a.ProductId == id);
            foreach (var imgs in img)
            {
               await _storage.DelFile(imgs.ImagePath);
            }

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

            //save img
            if (request.Thumnail != null)
            {
                var thumb = await _db.ProductImages.FirstOrDefaultAsync(a => a.IsDefault.Equals(true) && a.ProductId == request.Id);

                if (thumb != null)
                {
                    thumb.FileSize = request.Thumnail.Length;
                    thumb.ImagePath = await this.SaveImg(request.Thumnail);

                    _db.ProductImages.Update(thumb);
                }
            }

            return await _db.SaveChangesAsync();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(AdminProductPaging request)
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

        public async Task<string> SaveImg(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storage.SaveFile(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<List<ProductImageViewModel>> GetAllImg(int ProductId)
        {
            var result =  await _db.ProductImages.Where(a => a.ProductId == ProductId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
            return result;
        }

        public async Task<int> AddImg(ProductImageCreate create, int ProductId)
        {
            var productImage = new ProductImage()
            {
                Caption = create.Caption,
                DateCreated = DateTime.Now,
                IsDefault = create.IsDefault,
                ProductId = ProductId,
                SortOrder = create.SortOrder
            };

            if (create.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveImg(create.ImageFile);
                productImage.FileSize = create.ImageFile.Length;
            }
            _db.ProductImages.Add(productImage);
            await _db.SaveChangesAsync();
            return productImage.Id;
        }
    }
}
