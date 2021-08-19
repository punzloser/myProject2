using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Orders;

namespace ApiClientCommon.Service
{
    public class OrderApiClient : IOrderApiClient
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _accessor;

        public OrderApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _accessor = accessor;
        }

        public async Task<bool> AddOrderNew(OrderVm order)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(order);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/order", httpContent);

            if (response.IsSuccessStatusCode)
                return true;
            return false;

        }
    }
}
