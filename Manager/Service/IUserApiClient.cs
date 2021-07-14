using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Users;

namespace Manager.Service
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PageResult<UserViewModel>> GetUserPaging(UserPaging paging);
    }
}
