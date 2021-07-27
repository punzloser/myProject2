using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ViewModel.Catalog.Categories;

namespace Manager.Service
{
    public class CategoryApiClient : CommonApi, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor) : base(httpClient, accessor)
        {
        }
        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            string url = $"api/category?languageId={languageId}";
            return await GetTaskAsync<List<CategoryViewModel>>(url);
        }
    }
}
