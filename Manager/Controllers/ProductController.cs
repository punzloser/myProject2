using Manager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Products;

namespace Manager.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApi;
        public ProductController(IProductApiClient productApi)
        {
            _productApi = productApi;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 3)
        {
            //1. Goi check language = session
            var lang = HttpContext.Session.GetString("DefaultLangId");
            //2. Map paging view
            var result = new AdminProductPaging()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = lang
            };
            //3. Call api client
            var getApi = await _productApi.GetProductPaging(result);

            return View(getApi);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            //important
            if (!ModelState.IsValid)
                return View();
            var result = await _productApi.Create(request);
            if (result)
            {
                TempData["alert"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View();
        }
    }
}
