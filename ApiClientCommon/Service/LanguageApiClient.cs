using ApiClientCommon.Service;
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
    public class LanguageApiClient : CommonApi, ILanguageApiClient
    {
        public LanguageApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor, IConfiguration config) : base(httpClient, accessor, config)
        {
        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            return await GetTaskAsync<List<LanguageViewModel>>("/api/language");
        }
    }
}
