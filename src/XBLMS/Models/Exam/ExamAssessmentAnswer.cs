﻿using Datory;
using Datory.Annotations;

namespace XBLMS.Models
{
    [DataTable("exam_AssessmentAnswer")]
    public class ExamAssessmentAnswer : Entity
    {
        [DataColumn]
        public int UserId { get; set; }
        [DataColumn]
        public int ExamAssId { get; set; }
        [DataColumn]
        public int TmId { get; set; }
        [DataColumn]
        public string Answer { get; set; }
    }
}
