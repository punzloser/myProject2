using Application.Catalog.Products;
using Application.Catalog.ProductSlides;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductSlideService _productSlideService;

        public ProductController(IProductService productService, IProductSlideService productSlideService)
        {
            _productService = productService;
            _productSlideService = productSlideService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] AdminProductPaging request)
        {
            var result = await _productService.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("{categoryId}/paging")]
        public async Task<IActionResult> GetProductPagingByCategoryId(int categoryId, [FromQuery] AdminProductPaging request)
        {
            var result = await _productService.GetAllPagingByCategoryId(request, categoryId);
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
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var result = await _productService.Create(request);
            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update(int productId, [FromForm] ProductEditRequest request)
        {
            var result = await _productService.Edit(productId, request);

            if (result == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPut("{productId}/{newPrice}")]
        [Authorize]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var result = await _productService.UpdatePrice(productId, newPrice);
            if (result is true)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPost("{productId}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> EditImg([FromForm] ProductImageEdit edit, int ImgId)
        {
            var result = await _productService.EditImg(edit, ImgId);
            if (result == 0 || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("image/{ImgId}")]
        [Authorize]
        public async Task<IActionResult> DeleteImg(int ImgId)
        {
            var result = await _productService.DeleteImg(ImgId);
            if (result == 0 || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }

        [HttpGet("laptop/{languageId}/{quantity}")]
        public async Task<IActionResult> GetLaptopLatest(string languageId, int quantity)
        {
            var result = await _productService.GetLaptopLatest(languageId, quantity);
            return Ok(result);
        }

        [HttpGet("mobile/{languageId}/{quantity}")]
        public async Task<IActionResult> GetMobileLatest(string languageId, int quantity)
        {
            var result = await _productService.GetMobileLatest(languageId, quantity);
            return Ok(result);
        }

        [HttpGet("slide/{ProductId}")]
        public async Task<IActionResult> GetAllSlidesByProductId(int ProductId)
        {
            var result = await _productSlideService.GetAllByProductId(ProductId);
            return Ok(result);
        }

        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllProductByLanguage(string languageId)
        {
            var result = await _productService.GetAllProductByLanguage(languageId);
            return Ok(result);
        }
    }
}
