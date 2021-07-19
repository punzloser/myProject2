using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Users;

namespace Application.Catalog.Users
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<PageResult<UserViewModel>> GetUserPaging(UserPaging paging);
        Task<bool> EditUser(Guid id, UserEditModel userEditModel);
        Task<UserViewModel> GetUserById(Guid id);
        Task<bool> RemoveUser(Guid id);
    }
}
