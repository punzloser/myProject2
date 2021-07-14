using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;
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

        public async Task<PageResult<UserViewModel>> GetUserPaging(UserPaging paging)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            //add jwt token lấy ra lúc login thành công để gán vào header tên là Bearer để authorization.
            //Vì header này mà server mới biết là user nào đang request
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paging.Bearer);
            var response = await client.GetAsync($"api/user/paging?pageIndex={paging.PageIndex}&pageSize={paging.PageSize}&keyword={paging.Keyword}");
            
            var result = await response.Content.ReadAsStringAsync();
            var listUser = JsonConvert.DeserializeObject<PageResult<UserViewModel>>(result);

            return listUser;
        }
    }
}
