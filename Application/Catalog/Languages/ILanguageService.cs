using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Languages;

namespace Application.Catalog.Languages
{
    public interface ILanguageService
    {
        Task<List<LanguageViewModel>> GetAll();
    }
}
