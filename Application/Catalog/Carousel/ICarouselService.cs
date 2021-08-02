using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace Application.Catalog.Carousel
{
    public interface ICarouselService
    {
        Task<List<CarouselViewModel>> GetAll();
    }
}
