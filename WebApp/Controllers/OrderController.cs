using ApiClientCommon.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Carts;
using ViewModel.Catalog.Orders;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;

        public OrderController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }
        public IActionResult Index()
        {
            return View(GetCartStaging());
        }

        private CheckoutVm GetCartStaging()
        {
            var cartSession = HttpContext.Session.GetString("CartSession");
            var cartState = new List<CartVm>();
            if (cartSession != null)
            {
                cartState = JsonConvert.DeserializeObject<List<CartVm>>(cartSession);
            }

            var checkoutVm = new CheckoutVm()
            {
                OrderVm = new OrderVm(),
                Carts = cartState
            };
            return checkoutVm;
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutVm checkoutVm, string languagueId)
        {
            //checkoutVm get OrderVm
            //model get list of Carts
            var model = GetCartStaging();

            if (!ModelState.IsValid)
                return View(model);

            var orderDetail = new List<OrderDetailVm>();
            foreach (var item in model.Carts)
            {
                orderDetail.Add(new OrderDetailVm()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            var orderNew = new OrderVm()
            {
                LanguageId = CultureInfo.CurrentCulture.Name,
                ShipName = checkoutVm.OrderVm.ShipName,
                ShipAddress = checkoutVm.OrderVm.ShipAddress,
                ShipEmail = checkoutVm.OrderVm.ShipEmail,
                ShipPhoneNumber = checkoutVm.OrderVm.ShipPhoneNumber,
                OrderDetail = orderDetail
            };

            var result = await _orderApiClient.AddOrderNew(orderNew);
            if (result)
            {
                await _orderApiClient.SendEmailAsync(orderNew);

                TempData["alert"] = "Thông tin đã gửi cho người bán hàng.";

                HttpContext.Session.Remove("CartSession");

                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Lỗi");
                return View();
            }

        }
    }
}
