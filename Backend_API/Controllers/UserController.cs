using Application.Catalog.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Users;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.Register(request);
            if (!ModelState.IsValid || result == false)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var _token = await _userService.Authenticate(request);
            if (!ModelState.IsValid || string.IsNullOrEmpty(_token))
                return BadRequest();
            return Ok(_token);
        }

        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserPaging([FromQuery] UserPaging paging)
        {
            var result = await _userService.GetUserPaging(paging);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userService.GetUserById(id);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> EditUser(Guid id, [FromBody] UserEditModel userEditModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.EditUser(id, userEditModel);

            if (result)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
