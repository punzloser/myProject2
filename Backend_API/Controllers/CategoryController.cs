using Application.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Categories;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var result = await _categoryService.GetAll(languageId);
            return Ok(result);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> RoleAssign(int productId, [FromBody] CategoryEditModel categoryEditModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _categoryService.CategoryAssign(productId, categoryEditModel);

            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}
