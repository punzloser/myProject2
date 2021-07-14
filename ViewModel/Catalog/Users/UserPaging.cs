using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;

namespace ViewModel.Catalog.Users
{
    public class UserPaging : PageRequestBase
    {
        public string Keyword { get; set; }
    }
}
