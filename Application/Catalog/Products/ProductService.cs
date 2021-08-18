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
    public class ProductService : IProductService
    {
        private readonly MyDBContext _db;
        private readonly IStorageService _storage;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public ProductService(MyDBContext db, IStorageService storage)
        {
            _db = db;
            _storage = storage;
        }

        public async Task<string> SaveFromFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storage.SaveFile(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var language = _db.Languages;
            var translation = new List<ProductTranslation>();

            foreach (var lang in language)
            {
                if (lang.Id == request.LanguageId)
                {
                    translation.Add(new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        SeoAlias = request.SeoAlias,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    translation.Add(new ProductTranslation()
                    {
                        Name = "N/A",
                        LanguageId = lang.Id
                    });
                }
            }

            var product = new Data.Entity.Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = translation,
                IsFeatured = request.IsFeatured,
                ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumnail-img",
                        DateCreated = DateTime.Now,
                        FileSize = request.Thumnail == null? 0 : request.Thumnail.Length,
                        ImagePath = request.Thumnail == null? "": await this.SaveFromFile(request.Thumnail),
                        IsDefault = true,
                        SortOrder = 1
                    }
                }
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product.Id;
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

        public async Task<int> Edit(int productId, ProductEditRequest request)
        {
            var product = await _db.Products.FindAsync(productId);
            var productTranslation = await _db.ProductTranslations
                .FirstOrDefaultAsync(a => a.ProductId == productId && a.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null)
            {
                throw new CallException($"Không tìm thấy sản phẩm với id {productId}");
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
                var productImage = await _db.ProductImages.FirstOrDefaultAsync(a => a.ProductId == productId);
                if (productImage == null)
                {
                    var newImage = new ProductImage()
                    {
                        Caption = "Thumnail-img",
                        DateCreated = DateTime.Now,
                        FileSize = request.Thumnail.Length,
                        ImagePath = await this.SaveFromFile(request.Thumnail),
                        IsDefault = true,
                        SortOrder = 1,
                        ProductId = productId
                    };
                    await _db.ProductImages.AddAsync(newImage);
                }
                else
                {
                    await _storage.DelFile(productImage.ImagePath);

                    productImage.ImagePath = await this.SaveFromFile(request.Thumnail);
                    productImage.FileSize = request.Thumnail.Length;
                    _db.ProductImages.Update(productImage);
                }
            }

            return await _db.SaveChangesAsync();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(AdminProductPaging request)
        {
            //1 join
            var result = from a in _db.Products
                         join b in _db.ProductTranslations on a.Id equals b.ProductId
                         join c in _db.ProductCategories on a.Id equals c.ProductID into cs
                         from c in cs.DefaultIfEmpty()
                         join d in _db.Categories on c.CategoryID equals d.Id into ds
                         from d in ds.DefaultIfEmpty()
                         where b.LanguageId == request.LanguageId
                         select new
                         {
                             a,
                             b,
                             c
                         };
            //2 filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                result = result.Where(p => p.b.Name.Contains(request.Keyword));
            }

            if (request.CategoryId != null)
            {
                result = result.Where(p => p.c.CategoryID == request.CategoryId);
            }
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
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<PageResult<ProductViewModel>> GetAllPagingByCategoryId(AdminProductPaging request, int categoryId)
        {
            var result = from a in _db.Products
                         join b in _db.ProductTranslations on a.Id equals b.ProductId
                         join c in _db.ProductCategories on a.Id equals c.ProductID into cs
                         from c in cs.DefaultIfEmpty()
                         join d in _db.Categories on c.CategoryID equals d.Id into ds
                         from d in ds.DefaultIfEmpty()
                         join e in _db.ProductImages on a.Id equals e.ProductId into ce
                         from e in ce.DefaultIfEmpty()
                         where b.LanguageId == request.LanguageId && d.Id == categoryId
                         select new
                         {
                             a,
                             b,
                             c,
                             e
                         };

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                result = result.Where(p => p.b.Name.Contains(request.Keyword));
            }

            if (request.CategoryId != null)
            {
                result = result.Where(p => p.c.CategoryID == request.CategoryId);
            }

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
                    ViewCount = x.a.ViewCount,
                    Thumnail = x.e.ImagePath
                }).ToListAsync();

            var pageResult = new PageResult<ProductViewModel>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await (from p in _db.Products
                                 join pt in _db.ProductTranslations on p.Id equals pt.ProductId into ppt
                                 from pt in ppt.DefaultIfEmpty()
                                 join pc in _db.ProductCategories on p.Id equals pc.ProductID into ppc
                                 from pc in ppc.DefaultIfEmpty()
                                 join c in _db.Categories on pc.CategoryID equals c.Id into pcc
                                 from c in pcc.DefaultIfEmpty()
                                 join ct in _db.CategoryTranslations on c.Id equals ct.CategoryId into cct
                                 from ct in cct.DefaultIfEmpty()
                                 join pi in _db.ProductImages on p.Id equals pi.ProductId into ppi
                                 from pi in ppi.DefaultIfEmpty()
                                 where p.Id == productId && pt.LanguageId == languageId
                                 select new ProductViewModel()
                                 {
                                     Id = p.Id,
                                     Name = pt.Name,
                                     DateCreated = p.DateCreated,
                                     Description = pt.Description,
                                     Details = pt.Details,
                                     LanguageId = pt.LanguageId,
                                     OriginalPrice = p.OriginalPrice,
                                     Price = p.Price,
                                     SeoAlias = pt.SeoAlias,
                                     SeoDescription = pt.SeoDescription,
                                     SeoTitle = pt.SeoTitle,
                                     Stock = p.Stock,
                                     ViewCount = p.ViewCount,
                                     IsFeatured = p.IsFeatured,
                                     Thumnail = pi.ImagePath,
                                     ImgDetail = pi.ImageDetail,

                                     CategoryName = string.IsNullOrEmpty(ct.Name) ? null : ct.Name

                                 }).FirstOrDefaultAsync();


            return product;
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

        public async Task<List<ProductImageViewModel>> GetListImgById(int ProductId)
        {
            var result = await _db.ProductImages.Where(a => a.ProductId == ProductId)
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
                productImage.ImagePath = await this.SaveFromFile(create.ImageFile);
                productImage.FileSize = create.ImageFile.Length;
            }
            _db.ProductImages.Add(productImage);
            await _db.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> EditImg(ProductImageEdit edit, int ImgId)
        {
            var productImage = await _db.ProductImages.FindAsync(ImgId);

            if (productImage == null)
                throw new CallException($"Không tìm thấy hình ảnh với id {ImgId}");

            if (edit.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFromFile(edit.ImageFile);
                productImage.FileSize = edit.ImageFile.Length;
            }
            _db.ProductImages.Update(productImage);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteImg(int ImgId)
        {
            var productImage = await _db.ProductImages.FindAsync(ImgId);
            if (productImage == null)
                throw new CallException($"Không tìm thấy hình ảnh với id {ImgId}");

            _db.ProductImages.Remove(productImage);
            return await _db.SaveChangesAsync();
        }

        public async Task<ProductImageViewModel> GetImgById(int imgId)
        {
            var productImage = await _db.ProductImages.FindAsync(imgId);

            if (productImage == null)
                throw new CallException($"Không tồn tại hình ảnh nào với Id {imgId}");

            var result = new ProductImageViewModel()
            {
                Id = productImage.Id,
                Caption = productImage.Caption,
                DateCreated = productImage.DateCreated,
                FileSize = productImage.FileSize,
                ImagePath = productImage.ImagePath,
                IsDefault = productImage.IsDefault,
                ProductId = productImage.ProductId,
                SortOrder = productImage.SortOrder
            };

            return result;
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(PublicProductPaging request, string languageId)
        {
            //1 join
            var result = from a in _db.Products
                         join b in _db.ProductTranslations on a.Id equals b.ProductId
                         join c in _db.ProductCategories on a.Id equals c.ProductID
                         join d in _db.Categories on c.CategoryID equals d.Id
                         where b.LanguageId.Equals(languageId)
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
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<List<ProductViewModel>> GetLaptopLatest(string languageId, int quantity)
        {
            var query = await (from a in _db.Products
                               join b in _db.ProductTranslations on a.Id equals b.ProductId
                               join c in _db.ProductCategories on a.Id equals c.ProductID into cc
                               from c in cc.DefaultIfEmpty()
                               join d in _db.Categories on c.CategoryID equals d.Id into cd
                               from d in cd.DefaultIfEmpty()
                               join e in _db.ProductImages on a.Id equals e.ProductId into ec
                               from e in ec.DefaultIfEmpty()
                               where b.LanguageId == languageId && (e == null || e.IsDefault == true) && a.IsFeatured == true
                               && d.Id == 1 //1 laptop 2 mobile
                               orderby a.DateCreated descending
                               select new ProductViewModel()
                               {
                                   Id = a.Id,
                                   Name = b.Name,
                                   Price = a.Price,
                                   OriginalPrice = a.OriginalPrice,
                                   DateCreated = a.DateCreated,
                                   Description = b.Description,
                                   Details = b.Details,
                                   SeoAlias = b.SeoAlias,
                                   SeoTitle = b.SeoTitle,
                                   SeoDescription = b.SeoDescription,
                                   Stock = a.Stock,
                                   LanguageId = b.LanguageId,
                                   ViewCount = a.ViewCount,
                                   Thumnail = e.ImagePath
                               }).ToListAsync();

            var result = query.Take(quantity);

            return result.ToList();
        }

        public async Task<List<ProductViewModel>> GetMobileLatest(string languageId, int quantity)
        {
            var query = await (from a in _db.Products
                               join b in _db.ProductTranslations on a.Id equals b.ProductId
                               join c in _db.ProductCategories on a.Id equals c.ProductID into cc
                               from c in cc.DefaultIfEmpty()
                               join d in _db.Categories on c.CategoryID equals d.Id into cd
                               from d in cd.DefaultIfEmpty()
                               join e in _db.ProductImages on a.Id equals e.ProductId into ec
                               from e in ec.DefaultIfEmpty()
                               where b.LanguageId == languageId && (e == null || e.IsDefault == true) && a.IsFeatured == true
                               && d.Id == 2
                               orderby a.DateCreated descending
                               select new ProductViewModel()
                               {
                                   Id = a.Id,
                                   Name = b.Name,
                                   Price = a.Price,
                                   OriginalPrice = a.OriginalPrice,
                                   DateCreated = a.DateCreated,
                                   Description = b.Description,
                                   Details = b.Details,
                                   SeoAlias = b.SeoAlias,
                                   SeoTitle = b.SeoTitle,
                                   SeoDescription = b.SeoDescription,
                                   Stock = a.Stock,
                                   LanguageId = b.LanguageId,
                                   ViewCount = a.ViewCount,
                                   Thumnail = e.ImagePath
                               }).ToListAsync();

            var result = query.Take(quantity);

            return result.ToList();
        }

        public async Task<List<ProductViewModel>> GetAllProductByLanguage(string languageId)
        {
            var result = await (from p in _db.Products
                                join pt in _db.ProductTranslations on p.Id equals pt.ProductId into ppt
                                from pt in ppt.DefaultIfEmpty()
                                join pi in _db.ProductImages on p.Id equals pi.ProductId into ppi
                                from pi in ppi.DefaultIfEmpty()
                                where pt.LanguageId == languageId
                                select new ProductViewModel()
                                {
                                    Id = p.Id,
                                    Name = pt.Name,
                                    Price = p.Price,
                                    OriginalPrice = p.OriginalPrice,
                                    DateCreated = p.DateCreated,
                                    Description = pt.Description,
                                    Details = pt.Details,
                                    SeoAlias = pt.SeoAlias,
                                    SeoTitle = pt.SeoTitle,
                                    SeoDescription = pt.SeoDescription,
                                    Stock = p.Stock,
                                    LanguageId = pt.LanguageId,
                                    ViewCount = p.ViewCount,
                                    Thumnail = pi.ImagePath
                                }).ToListAsync();
            return result;
        }
    }
}
