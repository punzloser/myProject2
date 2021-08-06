using ApiClientCommon.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Products;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        public ICategoryApiClient _categoryApiClient { get; }

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
        public async Task<IActionResult> Category(int id, string culture)
        {
            var getCategoryById = await _categoryApiClient.GetById(id, culture);

            var productPaging = new AdminProductPaging()
            {
                CategoryId = id,
                LanguageId = culture,
                PageIndex = 1,
                PageSize = 5
            };
            var product = await _productApiClient.GetProductPagingByCategoryId(productPaging, id);

            return View(new ProductPaging()
            {
                Products = product,
                Category = getCategoryById
            });
        }
    }
}
