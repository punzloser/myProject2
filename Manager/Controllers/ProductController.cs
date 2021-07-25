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
            //3. Call api clientl
            var getApi = await _productApi.GetProductPaging(result);
            
            return View(getApi);
        }
    }
}
