﻿using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Route(Constants.ApiHomePrefix)]
    public partial class LoginController : ControllerBase
    {
        private const string Route = "login";
        private const string RouteCaptcha = "login/captcha";
        private const string RouteCheckCaptcha = "login/captcha/actions/check";
        private const string RouteSendSms = "login/actions/sendSms";

        private readonly ISettingsManager _settingsManager;
        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigRepository _configRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;
        private readonly IStatRepository _statRepository;

        public LoginController(ISettingsManager settingsManager, IAuthManager authManager, ICacheManager cacheManager, IConfigRepository configRepository, IUserRepository userRepository, ILogRepository logRepository, IStatRepository statRepository)
        {
            _settingsManager = settingsManager;
            _authManager = authManager;
            _cacheManager = cacheManager;
            _configRepository = configRepository;
            _userRepository = userRepository;
            _logRepository = logRepository;
            _statRepository = statRepository;
        }

        public class GetResult
        {
            public bool IsUserCaptchaDisabled { get; set; }
        }

        public class CheckRequest
        {
            public string Token { get; set; }
            public string Value { get; set; }
        }

        public class SubmitRequest
        {
            public string Account { get; set; }
            public string Password { get; set; }
            public string Mobile { get; set; }
            public bool IsPersistent { get; set; }
        }

        public class SubmitResult
        {
            public User User { get; set; }
            public string Token { get; set; }
        }
    }
}
