using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.ProductSlides;

namespace Application.Catalog.ProductSlides
{
    public class ProductSlideService : IProductSlideService
    {
        private readonly MyDBContext _db;
        public ProductSlideService(MyDBContext db)
        {
            _db = db;
        }

        public async Task<List<ProductSlideVm>> GetAllByProductId(int ProductId)
        {
            var result = await (from p in _db.Products
                                join ps in _db.ProductSlides on p.Id equals ps.ProductId into pps
                                from ps in pps.DefaultIfEmpty()
                                where p.Id == ProductId
                                select new ProductSlideVm()
                                {
                                    ImageProductSlide = ps.ImageProductSlide
                                }).ToListAsync();
            return result;
        }
    }
}
