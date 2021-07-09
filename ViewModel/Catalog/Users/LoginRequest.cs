using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Catalog.Users
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Pass { get; set; }
        public bool Remember { get; set; }
    }
}
