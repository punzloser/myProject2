using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Users;

namespace Application.Catalog.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _user;
        private readonly SignInManager<User> _signIn;
        private readonly RoleManager<Role> _role;
        private readonly IConfiguration _config;
        public UserService(UserManager<User> user, SignInManager<User> signIn, RoleManager<Role> role, IConfiguration config)
        {
            _user = user;
            _signIn = signIn;
            _role = role;
            _config = config;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _user.FindByNameAsync(request.UserName);
            if (user == null) return null;

            var result = await _signIn.PasswordSignInAsync(user, request.Pass, request.Remember, true);
            if (!result.Succeeded) return null;

            var role = await _user.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",role))
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var createUser = new User()
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Dob = request.Dob
            };
            var createPass = await _user.CreateAsync(createUser, request.Pass);

            if (createPass.Succeeded)
                return true;
            return false;

        }
    }
}
