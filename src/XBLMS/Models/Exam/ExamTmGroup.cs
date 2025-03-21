﻿using Datory;
using Datory.Annotations;
using System;
using System.Collections.Generic;
using XBLMS.Enums;

namespace XBLMS.Models
{
    [DataTable("exam_TmGroup")]
    public class ExamTmGroup : Entity
    {
        [DataColumn]
        public string GroupName { get; set; }
        [DataColumn]
        public TmGroupType GroupType { get; set; }
        [DataColumn(Text = true)]
        public List<int> TmIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> TreeIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> TxIds { get; set; }
        [DataColumn(Text = true)]
        public List<int> Nandus  { get; set; }
        [DataColumn(Text = true)]
        public List<string> Zhishidians { get; set; }
        [DataColumn(Text = true)]
        public string Description { get; set; }
        [DataColumn]
        public int TmTotal { get; set; }
        [DataColumn]
        public DateTime? DateFrom { get; set; }
        [DataColumn]
        public DateTime? DateTo { get; set; }
        [DataColumn]
        public bool Locked { get; set; } = false;
        [DataColumn]
        public bool OpenUser { get; set; } = false;
        [DataColumn]
        public List<int> OpenUserGroupIds { get; set; }

    }
}
