﻿using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin
{
    [OpenApiIgnore]
    [Route(Constants.ApiAdminPrefix)]
    public partial class SyncDatabaseController : ControllerBase
    {
        public const string Route = "syncDatabase";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly IDatabaseManager _databaseManager;
        private readonly IConfigRepository _configRepository;

        public SyncDatabaseController(ISettingsManager settingsManager, IAuthManager authManager, IDatabaseManager databaseManager, IConfigRepository configRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _databaseManager = databaseManager;
            _configRepository = configRepository;
        }

        public class GetResult
        {
            public string DatabaseVersion { get; set; }
            public string Version { get; set; }
        }

        public class SubmitRequest
        {
            public string SecurityKey { get; set; }
        }

        public class SubmitResult
        {
            public string Version { get; set; }
        }
    }
}