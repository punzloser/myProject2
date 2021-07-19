using Common.Exceptions;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;
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
            if (user == null) throw new CallException($"Không tìm thấy user {user.UserName}");

            var result = await _signIn.PasswordSignInAsync(user, request.Pass, request.Remember, true);
            if (!result.Succeeded) return null;

            var role = await _user.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",role)),
                new Claim(ClaimTypes.Name, request.UserName)
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

        public async Task<bool> EditUser(Guid id, UserEditModel userEditModel)
        {
            var check = await _user.Users.AnyAsync(a => a.Id != id && a.Email.CompareTo(userEditModel.Email) == 0);
            if (check)
                throw new CallException("Lỗi trùng email");

            var user = await _user.FindByIdAsync(id.ToString());
            user.Dob = userEditModel.Dob;
            user.Email = userEditModel.Email;
            user.FirstName = userEditModel.FirstName;
            user.LastName = userEditModel.LastName;
            user.PhoneNumber = userEditModel.PhoneNumber;

            var result = await _user.UpdateAsync(user);
            if (result.Succeeded)
                return true;
            return false;
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var user = await _user.FindByIdAsync(id.ToString());

            if (user == null)
                throw new CallException($"Không tìm thấy user {user.Id}");

            var result = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Dob = user.Dob,
                PhoneNumber = user.PhoneNumber
            };

            return result;
        }

        public async Task<PageResult<UserViewModel>> GetUserPaging(UserPaging paging)
        {
            var result = _user.Users;
            if (!string.IsNullOrEmpty(paging.Keyword))
                result = result.Where(a => a.UserName.Contains(paging.Keyword));

            int totalRow = await result.CountAsync();

            var item = await result.Select(a => new UserViewModel()
            {
                Id = a.Id,
                UserName = a.UserName,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                Dob = a.Dob,
                PhoneNumber = a.PhoneNumber
            }).Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();

            return new PageResult<UserViewModel>()
            {
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                TotalRecord = totalRow,
                Items = item,
            };
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

        public async Task<bool> RemoveUser(Guid id)
        {
            var user = await _user.FindByIdAsync(id.ToString());
            if (user == null)
                throw new CallException("User này không tồn tại");
            var result = await _user.DeleteAsync(user);
            if (!result.Succeeded)
                return false;
            return true;
        }
    }
}
