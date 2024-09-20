﻿using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Route(Constants.ApiAdminPrefix)]
    public partial class LogoutController : ControllerBase
    {
        public const string Route = "logout";

        private readonly IAuthManager _authManager;
        private readonly IDbCacheRepository _dbCacheRepository;

        public LogoutController(IAuthManager authManager, IDbCacheRepository dbCacheRepository)
        {
            _authManager = authManager;
            _dbCacheRepository = dbCacheRepository;
        }
    }
}