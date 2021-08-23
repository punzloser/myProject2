using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
        public CategoryApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor, IConfiguration config) : base(httpClient, accessor, config)
        {
            _httpClient = httpClient;
            _accessor = accessor;
            _config = config;
        }

        public async Task<bool> CategoryAssign(int ProductId, CategoryEditModel model)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_config["BaseApi"]);
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
