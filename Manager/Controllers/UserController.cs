using Manager.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Users;

namespace Manager.Controllers
{
    //[Route("login")]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _config;
        public UserController(IUserApiClient userApiClient, IConfiguration config)
        {
            _userApiClient = userApiClient;
            _config = config;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var request = new UserPaging()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };
            var data = await _userApiClient.GetUserPaging(request);
            return View(data);
        }

        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (ModelState.IsValid == false)
                return View();

            var result = await _userApiClient.Authenticate(request);
            if (result == "")
            {
                ModelState.AddModelError("", "Đăng nhập không hợp lệ");
                return View();
            }

            var userPrincipal = this.ValidateToken(result);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("token", result);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

            return RedirectToAction("Index", "User");

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("token");
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.Register(request);

            if (result)
            {
                TempData["alert"] = "Thêm thành công !";
                return RedirectToAction("Index");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetUserById(id);
            if (result != null)
            {
                var update = new UserEditModel()
                {
                    Dob = result.Dob,
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    PhoneNumber = result.PhoneNumber
                };

                return View(update);
            }

            return Content("Lỗi");

        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserEditModel userEditModel)
        {
            var result = await _userApiClient.EditUser(userEditModel.id, userEditModel);
            if (ModelState.IsValid)
            {
                TempData["alert"] = "Sửa thành công !";
                return RedirectToAction("Index");
            }
            return View(userEditModel);
        }

        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _userApiClient.RemoveUser(id);
            if (result)
            {
                TempData["alert"] = "Xóa thành công !";
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _config["Tokens:Issuer"];
            validationParameters.ValidIssuer = _config["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}
