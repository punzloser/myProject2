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
using ViewModel.Catalog.Roles;

namespace ApiClientCommon.Service
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _config;
        public RoleApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor, IConfiguration config)
        {
            _accessor = accessor;
            _httpClient = httpClient;
            _config = config;
        }
        public async Task<List<RoleViewModel>> GetAll()
        {
            var client = _httpClient.CreateClient();
            var session = _accessor.HttpContext.Session.GetString("token");
            client.BaseAddress = new Uri(_config["BaseApi"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync("/api/role");
            var result = await response.Content.ReadAsStringAsync();

            var listRole = JsonConvert.DeserializeObject<List<RoleViewModel>>(result);
            //
            if (response.IsSuccessStatusCode)
                return listRole;
            return null;

        }

        public async Task<bool> SetRoleAssign(Guid id, RoleEditModel roleEditModel)
        {
            var client = _httpClient.CreateClient();
            var session = _accessor.HttpContext.Session.GetString("token");
            client.BaseAddress = new Uri(_config["BaseApi"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var json = JsonConvert.SerializeObject(roleEditModel);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/role/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}
