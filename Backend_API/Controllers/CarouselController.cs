using Application.Catalog.Carousel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarouselController : ControllerBase
    {
        private readonly ICarouselService _carouselService;
        public CarouselController(ICarouselService carouselService)
        {
            _carouselService = carouselService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _carouselService.GetAll();
            return Ok(result);
        }

    }
}
