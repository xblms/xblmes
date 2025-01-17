﻿using Datory;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IOrganCompanyRepository : IRepository
    {
        Task<int> InsertAsync(OrganCompany company);

        Task<bool> UpdateAsync(OrganCompany company);

        Task<bool> DeleteAsync(int id);
        Task ClearAsync();
    }
}
