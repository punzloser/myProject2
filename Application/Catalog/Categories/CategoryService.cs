using Common.Exceptions;
using Data;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Categories;

namespace Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly MyDBContext _db;
        public CategoryService(MyDBContext db)
        {
            _db = db;
        }

        public async Task<bool> CategoryAssign(int ProductId, CategoryEditModel model)
        {
            var product = await _db.Products.FindAsync(ProductId);
            if (product == null)
                throw new CallException($"sản phẩm với id {ProductId} không tồn tại");

            foreach (var item in model.Categories)
            {
                var productCategory = await _db.ProductCategories
                    .FirstOrDefaultAsync(a => a.CategoryID == int.Parse(item.Id) && a.ProductID == product.Id);

                if (item.Checked && productCategory == null)
                {
                    await _db.ProductCategories.AddAsync(new ProductCategory()
                    {
                        CategoryID = int.Parse(item.Id),
                        ProductID = ProductId
                    });
                }
                else if (!item.Checked && productCategory != null)
                {
                    _db.ProductCategories.Remove(productCategory);

                }
            }
            var result = await _db.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var query = from a in _db.Categories
                        join b in _db.CategoryTranslations on a.Id equals b.CategoryId
                        where b.LanguageId == languageId
                        select new { a, b };
            var map = await query.Select(x => new CategoryViewModel()
            {
                Id = x.a.Id,
                Name = x.b.Name
            }).ToListAsync();

            return map;
        }
    }
}
