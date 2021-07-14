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
            return Ok();
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
    }
}
