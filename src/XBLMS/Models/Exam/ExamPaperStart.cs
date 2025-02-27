﻿using Datory;
using Datory.Annotations;
using System;

namespace XBLMS.Models
{
    [DataTable("exam_PaperStart")]
    public class ExamPaperStart : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int ExamPaperId { get; set; }
        [DataColumn]
        public int ExamPaperRandomId { get; set; }
        [DataColumn]
        public DateTime? BeginDateTime { get; set; }
        [DataColumn]
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// 已用时 秒
        /// </summary>
        [DataColumn]
        public long ExamTimeSeconds { get; set; }
        [DataColumn]
        public int MarkTeacherId { get; set; }
        [DataColumn]
        public bool IsMark { get; set; }
        [DataColumn]
        public DateTime? MarkDateTime { get; set; }
        [DataColumn]
        public decimal Score { get; set; }
        [DataColumn]
        public decimal SubjectiveScore { get; set; }
        [DataColumn]
        public decimal ObjectiveScore { get; set; }
        [DataColumn]
        public bool IsSubmit { get; set; }
        [DataColumn]
        public bool Locked { get; set; }
        [DataColumn]
        public int ExistCount { get; set; } = 0;
    }
}
