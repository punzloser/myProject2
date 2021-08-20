using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Orders;

namespace ApiClientCommon.Service
{
    public interface IOrderApiClient
    {
        Task<bool> AddOrderNew(OrderVm order);

        Task<bool> SendEmailAsync(OrderVm mailRequest);
    }
}
