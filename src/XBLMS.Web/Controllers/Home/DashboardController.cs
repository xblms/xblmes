﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Web.Controllers.Home
{
    [OpenApiIgnore]
    [Authorize(Roles = Types.Roles.User)]
    [Route(Constants.ApiHomePrefix)]
    public partial class DashboardController : ControllerBase
    {
        private const string Route = "dashboard";

        private readonly ISettingsManager _settingsManager;
        private readonly IConfigRepository _configRepository;
        private readonly IAuthManager _authManager;
        private readonly IOrganManager _organManager;
        private readonly IExamManager _examManager;
        private readonly IExamPaperUserRepository _examPaperUserRepository;
        private readonly IExamPaperRepository _examPaperRepository;
        private readonly IExamPaperStartRepository _examPaperStartRepository;
        private readonly IExamQuestionnaireRepository _examQuestionnaireRepository;
        private readonly IExamQuestionnaireUserRepository _examQuestionnaireUserRepository;
        private readonly IExamCerRepository _examCerRepository;
        private readonly IExamCerUserRepository _examCerUserRepository;

        private readonly IExamAssessmentUserRepository _examAssessmentUserRepository;
        private readonly IExamAssessmentRepository _examAssessmentRepository;

        private readonly IKnowlegesRepository _knowlegesRepository;

        public DashboardController(IConfigRepository configRepository,
            ISettingsManager settingsManager,
            IOrganManager organManager,
            IAuthManager authManager,
            IExamManager examManager,
            IExamPaperUserRepository examPaperUserRepository,
            IExamPaperRepository examPaperRepository,
            IExamPaperStartRepository examPaperStartRepository,
            IExamQuestionnaireRepository examQuestionnaireRepository,
            IExamQuestionnaireUserRepository examQuestionnaireUserRepository,
            IExamCerRepository examCerRepository,
            IExamCerUserRepository examCerUserRepository,
            IExamAssessmentUserRepository examAssessmentUserRepository,
            IExamAssessmentRepository examAssessmentRepository,
            IKnowlegesRepository knowlegesRepository)
        {
            _settingsManager = settingsManager;
            _configRepository = configRepository;
            _authManager = authManager;
            _organManager = organManager;
            _examManager = examManager;
            _examPaperUserRepository = examPaperUserRepository;
            _examPaperRepository = examPaperRepository;
            _examPaperStartRepository = examPaperStartRepository;
            _examQuestionnaireRepository = examQuestionnaireRepository;
            _examQuestionnaireUserRepository = examQuestionnaireUserRepository;
            _examCerRepository = examCerRepository;
            _examCerUserRepository = examCerUserRepository;
            _examAssessmentUserRepository = examAssessmentUserRepository;
            _examAssessmentRepository = examAssessmentRepository;
            _knowlegesRepository = knowlegesRepository;
        }

        public class GetRequest
        {
            public bool IsApp { get; set; }
        }

        public class GetResult
        {
            public User User { get; set; }
            public double AllPercent { get; set; }
            public double ExamPercent { get; set; }
            public double ExamMoniPercent { get; set; }
            public int ExamTotal { get; set; }
            public int ExamMoniTotal { get; set; }

            public ExamPaper ExamPaper { get; set; }
            public ExamPaper ExamMoni { get; set; }

            public int PracticeAnswerTmTotal { get; set; }
            public double PracticeAnswerPercent { get; set; }
            public int PracticeAllTmTotal { get; set; }
            public double PracticeAllPercent { get; set; }
            public int PracticeCollectTmTotal { get; set; }
            public double PracticeCollectPercent { get; set; }
            public int PracticeWrongTmTotal { get; set; }
            public double PracticeWrongPercent { get; set; }

            public int TaskPaperTotal { get; set; }
            public int TaskQTotal { get; set; }
            public int TaskAssTotal { get; set; }

            public ExamCerUser TopCer { get; set; }

            public string DateStr { get; set; }
            public List<ExamCerUser> CerList { get; set; }

            public List<Models.Knowledges> KnowList { get; set; }

            public ExamPaper TodayExam { get; set; }
            public List<ExamPaper> TaskPaperList { get; set; }
            public List<ExamQuestionnaire> TaskQList { get; set; }
            public List<ExamAssessment> TaskAssList { get; set; }
            public int TaskTotal { get; set; }
            public string Version { get; set; }

        }

    }
}
