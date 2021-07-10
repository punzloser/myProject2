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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _Pservice;
        private readonly IManageProductService _Mservice;
        public ProductController(IPublicProductService Pservice, IManageProductService Mservice)
        {
            _Pservice = Pservice;
            _Mservice = Mservice;
        }
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetPaging(string languageId, [FromQuery] PublicProductPaging request)
        {
            return Ok(await _Pservice.GetAllByCategoryId(request, languageId));
        }

        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var result = await _Mservice.GetById(productId, languageId);
            if (result == null)
                return NotFound("Không tìm thấy !");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var productId = await _Mservice.Create(request);
            if (productId == 0)
                return BadRequest();
            var product = await _Mservice.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductEditRequest request)
        {
            var result = await _Mservice.Edit(request);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPut("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var result = await _Mservice.UpdatePrice(productId, newPrice);
            if (result is true)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _Mservice.Delete(id);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddImg(int productId, [FromForm] ProductImageCreate create)
        {
            var ImgId = await _Mservice.AddImg(create, productId);
            var img = await _Mservice.GetImgById(ImgId);

            if (ImgId == 0 || !ModelState.IsValid)
                return BadRequest();
            return CreatedAtAction(nameof(GetImgById), new { id = productId }, img);
        }

        [HttpGet("{productId}/image/{imgId}")]
        public async Task<IActionResult> GetImgById(int imgId)
        {
            var result = await _Mservice.GetImgById(imgId);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPut("image/{ImgId}")]
        public async Task<IActionResult> EditImg([FromForm] ProductImageEdit edit, int ImgId)
        {
            var result = await _Mservice.EditImg(edit, ImgId);
            if (result == 0 || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("image/{ImgId}")]
        public async Task<IActionResult> DeleteImg(int ImgId)
        {
            var result = await _Mservice.DeleteImg(ImgId);
            if (result == 0 || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }
    }
}
