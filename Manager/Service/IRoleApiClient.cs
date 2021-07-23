using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Catalog.Roles;

namespace Manager.Service
{
    public interface IRoleApiClient
    {
        Task<List<RoleViewModel>> GetAll();
        Task<bool> SetRoleAssign(Guid id, RoleEditModel roleEditModel);
    }
}
