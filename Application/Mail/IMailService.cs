using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Orders;

namespace Application.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(OrderVm mailRequest);
    }
}
