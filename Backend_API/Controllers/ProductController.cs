using Application.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.ProductImages;
using ViewModel.Catalog.Products;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] AdminProductPaging request)
        {
            var result = await _productService.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var result = await _productService.GetById(productId, languageId);
            if (result == null)
                return NotFound("Không tìm thấy !");
            return Ok(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var productId = await _productService.Create(request);
            if (productId == 0)
                return BadRequest();
            var product = await _productService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductEditRequest request)
        {
            var result = await _productService.Edit(request);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPut("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var result = await _productService.UpdatePrice(productId, newPrice);
            if (result is true)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddImg(int productId, [FromForm] ProductImageCreate create)
        {
            var ImgId = await _productService.AddImg(create, productId);
            var img = await _productService.GetImgById(ImgId);

            if (ImgId == 0 || !ModelState.IsValid)
                return BadRequest();
            return CreatedAtAction(nameof(GetImgById), new { id = productId }, img);
        }

        [HttpGet("{productId}/image/{imgId}")]
        public async Task<IActionResult> GetImgById(int imgId)
        {
            var result = await _productService.GetImgById(imgId);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPut("image/{ImgId}")]
        public async Task<IActionResult> EditImg([FromForm] ProductImageEdit edit, int ImgId)
        {
            var result = await _productService.EditImg(edit, ImgId);
            if (result == 0 || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("image/{ImgId}")]
        public async Task<IActionResult> DeleteImg(int ImgId)
        {
            var result = await _productService.DeleteImg(ImgId);
            if (result == 0 || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("laptop/{languageId}/{quantity}")]
        public async Task<IActionResult> GetLaptopLatest(string languageId, int quantity)
        {
            var result = await _productService.GetLaptopLatest(languageId, quantity);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("mobile/{languageId}/{quantity}")]
        public async Task<IActionResult> GetMobileLatest(string languageId, int quantity)
        {
            var result = await _productService.GetMobileLatest(languageId, quantity);
            return Ok(result);
        }
    }
}
