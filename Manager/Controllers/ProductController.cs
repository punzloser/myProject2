using Manager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly ICategoryApiClient _categoryApi;
        public ProductController(IProductApiClient productApi, ICategoryApiClient categoryApi)
        {
            _productApi = productApi;
            _categoryApi = categoryApi;
        }

        public async Task<IActionResult> Index(int? categoryId, string keyword, int pageIndex = 1, int pageSize = 3)
        {
            //1. Goi check language = session
            var lang = HttpContext.Session.GetString("DefaultLangId");
            //2. Map paging view
            var result = new AdminProductPaging()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = lang,
                CategoryId = categoryId
            };
            //3. Call api client
            var getApi = await _productApi.GetProductPaging(result);
            //
            var getListCate = await _categoryApi.GetAll(lang);
            ViewBag.Category = getListCate.Select(a => new SelectListItem()
            {
                Text = a.Name,
                Value = a.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == a.Id
            });
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
