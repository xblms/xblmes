﻿using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Enums;
namespace XBLMS.Models
{
    [DataTable("exam_Assessment")]
    public class ExamAssessment : Entity
    {
        [DataColumn]
        public string Title { get; set; }
        [DataColumn]
        public int ConfigId { get; set; }
        [DataColumn]
        public DateTime? ExamBeginDateTime { get; set; } = DateTime.Now;
        [DataColumn]
        public DateTime? ExamEndDateTime { get; set; } = DateTime.Now.AddDays(3);

        [DataColumn(Text = true)]
        public List<int> UserGroupIds { get; set; }
        [DataColumn]
        public bool Locked { get; set; } = false;
        [DataColumn]
        public SubmitType SubmitType { get; set; } = SubmitType.Save;
        [DataColumn]
        public int AnswerTotal { get; set; } = 0;
    }
}