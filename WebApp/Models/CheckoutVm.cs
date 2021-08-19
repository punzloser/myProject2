using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Carts;
using ViewModel.Catalog.Orders;

namespace WebApp.Models
{
    public class CheckoutVm
    {
        public List<CartVm> Carts { get; set; }
        public OrderVm OrderVm { get; set; }
    }
}
