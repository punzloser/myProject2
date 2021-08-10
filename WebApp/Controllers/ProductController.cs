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
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IProductSlideApiClient _productSlideApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IProductSlideApiClient productSlideApiClient)
        {
            _productApiClient = productApiClient;
            _categoryApiClient = categoryApiClient;
            _productSlideApiClient = productSlideApiClient;
        }
        public async Task<IActionResult> Index(string culture)
        {
            var getAllProduct = await _productApiClient.GetAllProductByLanguage(culture);
            return View(getAllProduct);
        }

        public async Task<IActionResult> Detail(int id, string culture)
        {
            var getProductById = await _productApiClient.GetById(id, culture);
            var getProductSlides = await _productSlideApiClient.GetAllByProductId(id);

            return View(new ProductDetail()
            {
                Product = getProductById,
                ProductSlides = getProductSlides
            });
        }
        public async Task<IActionResult> Category(int id, string culture, int pageIndex = 1, int pageSize = 5)
        {
            var getCategoryById = await _categoryApiClient.GetById(id, culture);

            var productPaging = new AdminProductPaging()
            {
                CategoryId = id,
                LanguageId = culture,
                PageIndex = pageIndex,
                PageSize = pageSize
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
