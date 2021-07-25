using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Languages;

namespace Manager.Models
{
    public class NavViewModel
    {
        public List<LanguageViewModel> Languages { get; set; }
        public string CurrentLangId { get; set; }
    }
}
