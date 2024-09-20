﻿using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Services
{
    public partial interface IAuthManager
    {
        Task<User> GetUserAsync();

        Task<Administrator> GetAdminAsync();

        bool IsAdmin { get; }

        int AdminId { get; }

        string AdminName { get; }

        bool IsUser { get; }

        int UserId { get; }

        string UserName { get; }

        bool IsApi { get; }

        string ApiToken { get; }

        string GetCSRFToken();
    }
}