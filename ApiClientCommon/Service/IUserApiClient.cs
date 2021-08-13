﻿using Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Base;
using ViewModel.Catalog.Users;

namespace ApiClientCommon.Service
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PageResult<UserViewModel>> GetUserPaging(UserPaging paging);
        Task<CommonResult<bool>> Register(RegisterRequest register);
        Task<bool> EditUser(Guid id, UserEditModel userEditModel);
        Task<UserViewModel> GetUserById(Guid id);
        Task<bool> RemoveUser(Guid id);
    }
}
