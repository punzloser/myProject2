using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.Roles;

namespace Application.Catalog.Roles
{
    public interface IRoleService
    {
        Task<bool> RoleAssign(Guid id, RoleEditModel roleEditModel);
        Task<List<RoleViewModel>> GetAll();
    }
}
