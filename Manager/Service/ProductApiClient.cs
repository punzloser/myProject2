using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Products;

namespace Manager.Service
{
    public class ProductApiClient : CommonApi, IProductApiClient
    {
        public ProductApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor) : base(httpClient, accessor)
        {
        }
        public async Task<PageResult<ProductViewModel>> GetProductPaging(AdminProductPaging paging)
        {
            string url = $"api/product/paging?pageIndex={paging.PageIndex}&pageSize={paging.PageSize}" +
                $"&keyword={paging.Keyword}&languageId={paging.LanguageId}";

            return await GetTaskAsync<PageResult<ProductViewModel>>(url); 
        }
    }
}
