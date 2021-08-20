using Application.Catalog.Order;
using Application.Mail;
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
        private readonly IMailService _mailService;

        public OrderController(IOrderService orderService, IMailService mailService)
        {
            _orderService = orderService;
            _mailService = mailService;
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

        [HttpPost("send")]
        public async Task<IActionResult> SendMail(OrderVm request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
