using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Languages;

namespace Manager.Service
{
    public interface ILanguageApiClient
    {
        Task<List<LanguageViewModel>> GetAll();
    }
}
