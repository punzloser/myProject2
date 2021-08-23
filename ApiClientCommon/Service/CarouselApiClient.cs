using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace ApiClientCommon.Service
{
    public class CarouselApiClient : CommonApi, ICarouselApiClient
    {
        public CarouselApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor, IConfiguration config) : base(httpClient, accessor, config)
        {

        }
        public async Task<List<CarouselViewModel>> GetAll()
        {
            string url = "/api/Carousel";
            return await GetTaskAsync<List<CarouselViewModel>>(url);
        }
    }
}
