using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ApiClientCommon.Service;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISharedCultureLocalizer _loc;
        private readonly ICarouselApiClient _carouselApiClient;
        private readonly IProductApiClient _productApiClient;

        public HomeController(ILogger<HomeController> logger, ISharedCultureLocalizer loc, ICarouselApiClient carouselApiClient, IProductApiClient productApiClient)
        {
            _logger = logger;
            //_loc = loc;
            _carouselApiClient = carouselApiClient;
            _productApiClient = productApiClient;
        }
        
        public async Task<IActionResult> Index()
        {
            var lang = CultureInfo.CurrentCulture.Name;
            ViewBag.culture = lang;
            int takeQuantity = Common.Variables.WebApp.SetQuantityToTakeInCarousel;
            var laptopLatest = await _productApiClient.GetLaptopLatest(lang, takeQuantity);
            var mobleLatest = await _productApiClient.GetMobileLatest(lang, takeQuantity);
            var carousel = await _carouselApiClient.GetAll();
            var result = new HomeViewModel()
            {
                Carousels = carousel,
                LaptopLatest = laptopLatest,
                MobileLatest = mobleLatest
            };
            return View(result);
        }

        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
