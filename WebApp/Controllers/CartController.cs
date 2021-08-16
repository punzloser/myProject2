using ApiClientCommon.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        public CartController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddCart(int id, string languageId)
        {

            var cartSession = HttpContext.Session.GetString("CartSession");

            var product = await _productApiClient.GetById(id, languageId);

            var cartState = new List<CartVm>();
            if (cartSession != null)
                cartState = JsonConvert.DeserializeObject<List<CartVm>>(cartSession);

            int quantity = 1;
            if (cartState.Any(x => x.ProductId == id))
            {
                if (cartState.Any(a => a.Quantity >= 10))
                {
                    cartState.First(a => a.ProductId == id).Quantity = 0;
                }
                cartState.First(a => a.ProductId == id).Quantity += quantity;
            }
            else
            {
                var cartItem = new CartVm()
                {
                    ProductId = id,
                    Description = product.Description,
                    Img = product.Thumnail,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                };
                cartState.Add(cartItem);
            }

            HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(cartState));
            return Ok(cartState);
        }

        public IActionResult GetList()
        {
            var cartSession = HttpContext.Session.GetString("CartSession");
            var cartState = new List<CartVm>();

            if (cartSession != null)
                cartState = JsonConvert.DeserializeObject<List<CartVm>>(cartSession);

            return Ok(cartState);
        }
    }
}
