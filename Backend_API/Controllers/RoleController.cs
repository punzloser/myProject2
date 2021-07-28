using Application.Catalog.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Roles;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _role;
        public RoleController(IRoleService role)
        {
            _role = role;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _role.GetAll();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleEditModel roleEditModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await _role.RoleAssign(id, roleEditModel);

            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}
