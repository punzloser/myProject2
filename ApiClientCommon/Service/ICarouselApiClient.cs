using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace ApiClientCommon.Service
{
    public interface ICarouselApiClient
    {
        Task<List<CarouselViewModel>> GetAll();
    }
}
