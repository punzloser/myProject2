using ApiClientCommon.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Categories;
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var lang = HttpContext.Session.GetString("DefaultLangId");
            var product = await _productApi.GetById(id, lang);

            var result = new ProductEditRequest()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Details = product.Details,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle
            };

            return View(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ProductEditRequest request)
        {
            //important
            if (!ModelState.IsValid)
                return View();
            var result = await _productApi.Edit(request);
            if (result)
            {
                TempData["alert"] = "Sửa sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Sửa sản phẩm thất bại");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var categoryAssign = await GetCategoryAssign(id);
            if (categoryAssign is null)
                return Content("Không có quyền truy cập");
            return View(categoryAssign);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _categoryApi.CategoryAssign(categoryEditModel.Id, categoryEditModel);
            if (result)
            {
                TempData["alert"] = "Sửa loại sản phẩm thành công !";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Sửa loại sản phẩm thất bại !");
            var categoryAssign = await GetCategoryAssign(categoryEditModel.Id);

            return View(categoryAssign);

        }

        private async Task<CategoryEditModel> GetCategoryAssign(int id)
        {
            var languageId = HttpContext.Session.GetString("DefaultLangId");
            var product = await _productApi.GetById(id, languageId);
            var categories = await _categoryApi.GetAll(languageId);

            var categoryAssign = new CategoryEditModel();
            foreach (var item in categories)
            {
                categoryAssign.Categories.Add(new Item()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Checked = product.Categories.Contains(item.Name)
                });
            }
            return categoryAssign;
        }
    }
}
