using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace ViewModel.Catalog.Products
{
    public class AdminProductPaging : PageRequestBase
    {
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; }
        //Truyền lên LanguageId để biết đang quản lí ngôn ngữ nào
        public string LanguageId { get; set; }
    }
}
