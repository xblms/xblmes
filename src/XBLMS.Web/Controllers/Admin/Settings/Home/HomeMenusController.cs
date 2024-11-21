﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Settings.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class HomeMenusController : ControllerBase
    {
        private const string Route = "settings/homeMenus";
        private const string RouteDelete = "settings/homeMenus/actions/delete";
        private const string RouteReset = "settings/homeMenus/actions/reset";

        private readonly IAuthManager _authManager;
        private readonly IUserMenuRepository _userMenuRepository;

        public HomeMenusController(IAuthManager authManager, IUserMenuRepository userMenuRepository)
        {
            _authManager = authManager;
            _userMenuRepository = userMenuRepository;
        }

        public class GetResult
        {
            public List<UserMenu> UserMenus { get; set; }
        }

        public class UserMenusResult
        {
            public List<UserMenu> UserMenus { get; set; }
        }

        public class SubmitRequest
        {
            public int Id { get; set; }
            public bool Disabled { get; set; }
            public int ParentId { get; set; }
            public int Taxis { get; set; }
            public string Text { get; set; }
            public string IconClass { get; set; }
            public string Link { get; set; }
            public string Target { get; set; }
        }
    }
}
