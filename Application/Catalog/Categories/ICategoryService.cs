using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Categories;

namespace Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);
        Task<bool> CategoryAssign(int ProductId, CategoryEditModel model);
        Task<CategoryViewModel> GetById(int categoryId, string languageId);

    }
}
