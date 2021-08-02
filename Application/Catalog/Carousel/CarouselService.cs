using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace Application.Catalog.Carousel
{
    public class CarouselService : ICarouselService
    {
        private readonly MyDBContext _db;
        public CarouselService(MyDBContext db)
        {
            _db = db;
        }
        public async Task<List<CarouselViewModel>> GetAll()
        {
            var result = await (from a in _db.Carousels
                                orderby a.SortOrder descending
                                select new CarouselViewModel
                                {
                                    Id = a.Id,
                                    Alt = a.Alt,
                                    Href = a.Href,
                                    Source = a.Source
                                }).ToListAsync();

            return result;
        }
    }
}
