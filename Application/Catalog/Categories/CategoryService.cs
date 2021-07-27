using Data;
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
