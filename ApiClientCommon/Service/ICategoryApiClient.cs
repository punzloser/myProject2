using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Categories;

namespace ApiClientCommon.Service
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);
        Task<bool> CategoryAssign(int ProductId, CategoryEditModel model);
        Task<CategoryViewModel> GetById(int categoryId, string languageId);
    }
}
