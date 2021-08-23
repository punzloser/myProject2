using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Catalog.Languages;

namespace ApiClientCommon.Service
{
    public class CommonApi
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;
        protected CommonApi(IHttpClientFactory httpClient, IHttpContextAccessor accessor, IConfiguration config)
        {
            _httpClient = httpClient;
            _accessor = accessor;
            _config = config;
        }

        protected async Task<TResult> GetTaskAsync<TResult>(string url)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_config["BaseApi"]);
            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<TResult>(result);
            return default;
        }

        protected async Task<bool> DeleteTaskAsync(string url)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri(_config["BaseApi"]);
            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

    }
}
