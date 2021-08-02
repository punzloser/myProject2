using Microsoft.AspNetCore.Http;
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
        public CarouselApiClient(IHttpClientFactory httpClient, IHttpContextAccessor accessor) : base(httpClient, accessor)
        {

        }
        public async Task<List<CarouselViewModel>> GetAll()
        {
            string url = "api/Carousel";
            return await GetTaskAsync<List<CarouselViewModel>>(url);
        }
    }
}
