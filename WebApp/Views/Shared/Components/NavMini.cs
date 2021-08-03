using ApiClientCommon.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Views.Shared.Components
{
    public class NavMini : ViewComponent
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public NavMini(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lang = CultureInfo.CurrentCulture.Name;
            var result = await _categoryApiClient.GetAll(lang);
            return View(result);
        }
    }
}
