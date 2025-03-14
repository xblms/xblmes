﻿using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IKnowlegesTreeRepository
    {
        Task<List<string>> GetParentPathAsync(int id);
        Task<List<KnowledgesTree>> GetListAsync();
        Task<KnowledgesTree> GetAsync(int id);
        Task<List<int>> GetIdsAsync(int id);
        Task<List<KnowledgesTree>> GetChildAsync(int parentId);
    }
}
