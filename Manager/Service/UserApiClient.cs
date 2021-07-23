using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _accessor;
        public UserApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _accessor = accessor;
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

            //work ok
            if (response.IsSuccessStatusCode)
                return result;
            return "";
        }

        public async Task<bool> EditUser(Guid id, UserEditModel userEditModel)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            var session = _accessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(userEditModel);
            var httpcontent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/user/{id}", httpcontent);

            var result = await response.Content.ReadAsStringAsync();

            if (result != "")
                return true;
            return false;
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var client = _httpClient.CreateClient();
            var session = _accessor.HttpContext.Session.GetString("token");
            client.BaseAddress = new Uri("https://localhost:5001");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"api/user/{id}");

            var result = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(result);
            return user;
        }

        public async Task<PageResult<UserViewModel>> GetUserPaging(UserPaging paging)
        {
            var client = _httpClient.CreateClient();
            var session = _accessor.HttpContext.Session.GetString("token");
            client.BaseAddress = new Uri("https://localhost:5001");

            //add jwt token lấy ra lúc login thành công để gán vào header tên là Bearer để authorization.
            //Vì header này mà server mới biết là user nào đang request
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"api/user/paging?pageIndex={paging.PageIndex}&pageSize={paging.PageSize}&keyword={paging.Keyword}");

            var result = await response.Content.ReadAsStringAsync();
            var listUser = JsonConvert.DeserializeObject<PageResult<UserViewModel>>(result);

            return listUser;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var client = _httpClient.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/user/register", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveUser(Guid id)
        {
            var client = _httpClient.CreateClient();
            var session = _accessor.HttpContext.Session.GetString("token");
            client.BaseAddress = new Uri("https://localhost:5001");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.DeleteAsync($"api/user/{id}");

            var result = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject(result);
            if (user == null)
                return false;
            return true;
        }
    }
}
