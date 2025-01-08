﻿using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IUserGroupRepository : IRepository
    {
        Task<int> InsertAsync(UserGroup group);
        Task UpdateAsync(UserGroup group);
        Task DeleteByUserId(int userId);
        Task DeleteAsync(int groupId);
        Task ClearCache();
        Task ResetAsync();
    }
}
