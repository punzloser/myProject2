using Common.Exceptions;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Roles;

namespace Application.Catalog.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _role;
        private readonly UserManager<User> _user;
        public RoleService(RoleManager<Role> role, UserManager<User> user)
        {
            _role = role;
            _user = user;
        }
        public async Task<List<RoleViewModel>> GetAll()
        {
            var role = await _role.Roles.Select(a => new RoleViewModel()
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToListAsync();

            return role;
        }
        public async Task<bool> RoleAssign(Guid id, RoleEditModel roleEditModel)
        {
            var user = await _user.FindByIdAsync(id.ToString());
            if (user == null)
                throw new CallException($"Tài khoản {roleEditModel.Roles.Select(a => a.Name)} không tồn tại");

            var userRole = await _user.GetRolesAsync(user);

            if(roleEditModel.Roles != null)
            {
                foreach (var role in roleEditModel.Roles)
                {
                    if(role.Checked && !userRole.Contains(role.Name))
                    {
                        await _user.AddToRoleAsync(user, role.Name);
                    }
                    else if (!role.Checked && userRole.Contains(role.Name))
                    {
                        await _user.RemoveFromRoleAsync(user, role.Name);
                    }
                }
            }
            return true;
        }
    }
}
