using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Catalog.Users
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Pass { get; set; }
        public bool Remember { get; set; }
    }
}
