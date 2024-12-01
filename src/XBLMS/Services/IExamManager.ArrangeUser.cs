﻿using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IExamManager
    {
        Task Arrange(ExamPaper paper);
        Task Arrange(int paperId, int userId);
        Task<List<int>> GetUserIdsByUserGroups(List<int> userGroupIds);
    }

}
