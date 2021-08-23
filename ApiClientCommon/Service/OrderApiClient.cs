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
using ViewModel.Catalog.Orders;

namespace ApiClientCommon.Service
{
    public class OrderApiClient : IOrderApiClient
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;

        public OrderApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor, IConfiguration config)
        {
            _httpClient = httpClient;
            _accessor = accessor;
            _config = config;
        }

        public async Task<bool> AddOrderNew(OrderVm order)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_config["BaseApi"]);
            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(order);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/order", httpContent);

            if (response.IsSuccessStatusCode)
                return true;
            return false;

        }

        public async Task<bool> SendEmailAsync(OrderVm mailRequest)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_config["BaseApi"]);

            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var json = JsonConvert.SerializeObject(mailRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/order/send", httpContent);

            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}
