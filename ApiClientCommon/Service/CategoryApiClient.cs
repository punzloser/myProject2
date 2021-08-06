using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Categories;

namespace ApiClientCommon.Service
{
    public class CategoryApiClient : CommonApi, ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _accessor;
        public CategoryApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor) : base(httpClient, accessor)
        {
            _httpClient = httpClient;
            _accessor = accessor;
        }

        public async Task<bool> CategoryAssign(int ProductId, CategoryEditModel model)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/category/{ProductId}", httpContent);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            string url = $"/api/category?languageId={languageId}";
            return await GetTaskAsync<List<CategoryViewModel>>(url);
        }

        public async Task<CategoryViewModel> GetById(int categoryId, string languageId)
        {
            string url = $"/api/category/{languageId}/{categoryId}";
            return await GetTaskAsync<CategoryViewModel>(url);
        }
    }
}
