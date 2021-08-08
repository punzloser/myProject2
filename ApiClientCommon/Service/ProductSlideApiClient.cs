﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.ProductSlides;

namespace ApiClientCommon.Service
{
    public class ProductSlideApiClient : CommonApi, IProductSlideApiClient
    {
        public ProductSlideApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor) : base(httpClient, accessor)
        {

        }

        public async Task<List<ProductSlideVm>> GetAllByProductId(int ProductId)
        {
            string url = $"api/product/slide/{ProductId}";
            return await GetTaskAsync<List<ProductSlideVm>>(url);
        }
    }
}
