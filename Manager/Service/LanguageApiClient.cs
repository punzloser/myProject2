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
    public class LanguageApiClient : CommonApi, ILanguageApiClient
    {
        public LanguageApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor) : base(httpClient, accessor)
        {
        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            return await GetTaskAsync<List<LanguageViewModel>>("api/language");
        }
    }
}
