using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ViewModel.Catalog.Languages;

namespace Manager.Service
{
    public class LanguageApiClient : ILanguageApiClient
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _accessor;
        public LanguageApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _accessor = accessor;
        }
        public async Task<List<LanguageViewModel>> GetAll()
        {
            var client =  _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync("api/language");
            var result = await response.Content.ReadAsStringAsync();
            var listLang = JsonConvert.DeserializeObject<List<LanguageViewModel>>(result);

            if (response.IsSuccessStatusCode)
                return listLang;
            return null;
        }
    }
}
