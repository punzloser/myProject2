using Manager.Models;
using Manager.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Views.Shared.Components
{
    public class Nav : ViewComponent
    {
        private readonly ILanguageApiClient _languageApi;
        public Nav(ILanguageApiClient languageApi)
        {
            _languageApi = languageApi;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lang = await _languageApi.GetAll();
            var nav = new NavViewModel()
            {
                CurrentLangId = HttpContext.Session.GetString("DefaultLangId"),
                Languages = lang
            };
            return View("Default", nav);
        }
    }
}
