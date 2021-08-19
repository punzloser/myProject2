using Data;
using Data.Entity;
using Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Orders;

namespace Application.Catalog.Order
{
    public class OrderService : IOrderService
    {
        private readonly MyDBContext _db;

        public OrderService(MyDBContext db)
        {
            _db = db;
        }
        public async Task<bool> AddOrderNew(OrderVm request)
        {
            var details = new List<OrderDetail>();

            foreach (var item in request.OrderDetail)
            {
                details.Add(new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                });
            }
            
            var orderNew = new Data.Entity.Order()
            {
                UserId =  null,
                ShipName = request.ShipName,
                ShipAddress = request.ShipAddress,
                ShipEmail = request.ShipEmail,
                ShipPhoneNumber = request.ShipPhoneNumber,
                OrderDate = DateTime.Now,
                Status = OrderStatus.InProgress,
                OrderDetails = details
            };

            await _db.Orders.AddAsync(orderNew);

            return await _db.SaveChangesAsync() > 0;
        }
    }
}
