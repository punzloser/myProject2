using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Users;

namespace Manager.Service
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClient;
        public UserApiClient(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            var json = JsonConvert.SerializeObject(request);

            //fromform mediaType : multipart/form-data
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/user/login", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
