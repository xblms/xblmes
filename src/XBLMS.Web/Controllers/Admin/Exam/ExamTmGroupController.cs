﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.Administrator)]
    [Route(Constants.ApiAdminPrefix)]
    public partial class ExamTmGroupController : ControllerBase
    {
        private const string Route = "exam/examTmGroup";
        private const string RouteDelete = Route + "/delete";

        private const string RouteEditGet = Route + "/editGet";
        private const string RouteEditPost = Route + "/editPost";

        private readonly IAuthManager _authManager;
        private readonly ICacheManager _cacheManager;
        private readonly IConfigRepository _configRepository;
        private readonly IExamTmGroupRepository _examTmGroupRepository;
        private readonly IExamTxRepository _examTxRepository;
        private readonly IExamManager _examManager;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IExamTmRepository _examTmRepository;

        public ExamTmGroupController(IAuthManager authManager, ICacheManager cacheManager, IConfigRepository configRepository, IExamTmGroupRepository examTmGroupRepository, IExamManager examManager, IExamTxRepository examTxRepository, IAdministratorRepository administratorRepository, IExamTmRepository examTmRepository)
        {
            _authManager = authManager;
            _cacheManager = cacheManager;
            _configRepository = configRepository;
            _examTmGroupRepository = examTmGroupRepository;
            _examManager = examManager;
            _examTxRepository = examTxRepository;
            _administratorRepository = administratorRepository;
            _examTmRepository = examTmRepository;
        }
        public class GetRequest
        {
            public string Search { get; set; }
        }
        public class GetResult
        {
            public IEnumerable<ExamTmGroup> Groups { get; set; }
        }


        public class GetEditResult
        {
            public ExamTmGroup Group { get; set; }
            public List<Cascade<int>> TmTree { get; set; }
            public List<ExamTx> TxList { get; set; }
            public List<Select<string>> GroupTypeSelects { get; set; }
        }

        public class GetEditRequest
        {
            public ExamTmGroup Group { get; set; }
        }


    }
}
