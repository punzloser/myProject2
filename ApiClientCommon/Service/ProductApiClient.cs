using ApiClientCommon.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Products;

namespace ApiClientCommon.Service
{
    public class ProductApiClient : CommonApi, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;
        public ProductApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor, IConfiguration config) : base(httpClient, accessor, config)
        {
            _httpClient = httpClient;
            _accessor = accessor;
            _config = config;
        }

        public async Task<bool> Create(ProductCreateRequest request)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_config["BaseApi"]);
            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            if (request.Thumnail != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Thumnail.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Thumnail.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumnail", request.Thumnail.FileName);
            }
            var langId = _accessor.HttpContext.Session.GetString("DefaultLangId");

            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Price.ToString()) ? "" : request.Price.ToString()), "price");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.OriginalPrice.ToString()) ? "" : request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Stock.ToString()) ? "" : request.Stock.ToString()), "stock");

            if (request.SeoDescription != null) { requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription"); }
            if (request.Details != null) { requestContent.Add(new StringContent(request.Details.ToString()), "details"); }
            if (request.SeoAlias != null) { requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias"); }
            if (request.Description != null) { requestContent.Add(new StringContent(request.Description.ToString()), "description"); }
            if (request.SeoTitle != null) { requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle"); }
            
            requestContent.Add(new StringContent(langId), "languageId");

            var response = await client.PostAsync("/api/product", requestContent);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> Delete(int productId)
        {
            string url = $"/api/product/{productId}";
            return await DeleteTaskAsync(url);
        }

        public async Task<bool> Edit(ProductEditRequest request)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_config["BaseApi"]);
            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            if (request.Thumnail != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Thumnail.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Thumnail.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumnail", request.Thumnail.FileName);
            }
            var langId = _accessor.HttpContext.Session.GetString("DefaultLangId");

            if (request.SeoDescription != null) { requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription"); }
            if (request.Details != null) { requestContent.Add(new StringContent(request.Details.ToString()), "details"); }
            if (request.SeoAlias != null) { requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias"); }
            if (request.Description != null) { requestContent.Add(new StringContent(request.Description.ToString()), "description"); }
            if (request.SeoTitle != null) { requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle"); }

            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(langId), "languageId");

            var response = await client.PutAsync($"/api/product/{request.Id}", requestContent);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<List<ProductViewModel>> GetAllProductByLanguage(string languageId)
        {
            string url = $"api/product/{languageId}";
            return await GetTaskAsync<List<ProductViewModel>>(url);
        }

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            string url = $"/api/product/{productId}/{languageId}";
            return await GetTaskAsync<ProductViewModel>(url);
        }

        public async Task<List<ProductViewModel>> GetLaptopLatest(string languageId, int quantity)
        {
            string url = $"/api/product/laptop/{languageId}/{quantity}";
            return await GetTaskAsync<List<ProductViewModel>>(url);
        }

        public async Task<List<ProductViewModel>> GetMobileLatest(string languageId, int quantity)
        {
            string url = $"/api/product/mobile/{languageId}/{quantity}";
            return await GetTaskAsync<List<ProductViewModel>>(url);
        }

        public async Task<PageResult<ProductViewModel>> GetProductPaging(AdminProductPaging paging)
        {
            string url = $"/api/product/paging?pageIndex={paging.PageIndex}&pageSize={paging.PageSize}" +
                $"&keyword={paging.Keyword}&languageId={paging.LanguageId}&categoryId={paging.CategoryId}";

            return await GetTaskAsync<PageResult<ProductViewModel>>(url);
        }

        public async Task<PageResult<ProductViewModel>> GetProductPagingByCategoryId(AdminProductPaging paging, int categoryId)
        {
            string url = $"/api/product/{categoryId}/paging?pageIndex={paging.PageIndex}&pageSize={paging.PageSize}" +
                $"&keyword={paging.Keyword}&languageId={paging.LanguageId}&categoryId={paging.CategoryId}";

            return await GetTaskAsync<PageResult<ProductViewModel>>(url);
        }
    }
}
