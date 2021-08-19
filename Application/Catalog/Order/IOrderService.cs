using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Orders;

namespace Application.Catalog.Order
{
    public interface IOrderService
    {
        public Task<bool> AddOrderNew(OrderVm order);
    }
}
