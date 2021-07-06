using Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _service;
        public ProductController(IPublicProductService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAll());
        }
    }
}
