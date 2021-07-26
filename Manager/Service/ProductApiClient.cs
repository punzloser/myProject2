using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Products;

namespace Manager.Service
{
    public class ProductApiClient : CommonApi, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _accessor;
        public ProductApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor) : base(httpClient, accessor)
        {
            _httpClient = httpClient;
            _accessor = accessor;
        }

        public async Task<bool> Create(ProductCreateRequest request)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
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
                requestContent.Add(bytes, "thumbnailImage", request.Thumnail.FileName);
            }
            var langId = _accessor.HttpContext.Session.GetString("DefaultLangId");

            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Details.ToString()), "details");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(langId), "languageId");

            var response = await client.PostAsync("api/product", requestContent);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<PageResult<ProductViewModel>> GetProductPaging(AdminProductPaging paging)
        {
            string url = $"api/product/paging?pageIndex={paging.PageIndex}&pageSize={paging.PageSize}" +
                $"&keyword={paging.Keyword}&languageId={paging.LanguageId}";

            return await GetTaskAsync<PageResult<ProductViewModel>>(url);
        }
    }
}
