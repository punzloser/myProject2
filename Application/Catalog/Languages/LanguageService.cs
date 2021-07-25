using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Languages;

namespace Application.Catalog.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly MyDBContext _db;
        public LanguageService(MyDBContext db)
        {
            _db = db;
        }
        public async Task<List<LanguageViewModel>> GetAll()
        {
            var lang = await _db.Languages.Select(a => new LanguageViewModel()
            {
                Id = a.Id,
                Name = a.Name
            })
                .OrderByDescending(a=>a.Name)
                .ToListAsync();
            return lang;
        }
    }
}
