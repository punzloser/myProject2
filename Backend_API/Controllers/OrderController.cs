using Application.Catalog.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Orders;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddOrderNew(OrderVm request)
        {
            var result = await _orderService.AddOrderNew(request);
            if (!result)
                return BadRequest();
            return Ok();
        }
    }
}
