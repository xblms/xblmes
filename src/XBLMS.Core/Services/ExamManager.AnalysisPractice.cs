﻿using System;
using System.Threading.Tasks;

namespace XBLMS.Core.Services
{
    public partial class ExamManager
    {
        public async Task<(int answerTmTotal,double answerPercent,int allTmTotal,double allAnswerPercent,int collectTmTotal,double collectAnswerPercent,int wrongTmTotal,double wrongAnswerPercent)> AnalysisPractice(int userId)
        {
            var tmGroups = await _examTmGroupRepository.GetListWithoutLockedAsync();


            double answerPercent = 0;

            var allTmTotal = 0;
            double allPercent = 0;

            var collectTmTotal = 0;
            double collectPercent = 0;

            var wrongTmTotal = 0;
            double wrongPercent = 0;

            if (tmGroups != null && tmGroups.Count > 0)
            {
                foreach (var tmGroup in tmGroups)
                {
                    if (tmGroup.OpenUser)
                    {
                        var total = await _examTmRepository.GetCountAsync(tmGroup, null, 0, 0, "", "", "", false, 0, int.MaxValue);
                        allTmTotal += total;
                    }
                }
            }

            var (answerTotal,rightTotal,allAnswerTotal,allRightTotal,collectAnswerTotal,collectRightTotal,wrongAnswerTotal,wrongRightTotal) = await _examPracticeRepository.SumAsync(userId);



            var collect = await _examPracticeCollectRepository.GetAsync(userId);
            if (collect != null && collect.TmIds != null)
            {
                collectTmTotal = collect.TmIds.Count;
            }

            var wrong = await _examPracticeWrongRepository.GetAsync(userId);
            if (wrong != null && wrong.TmIds != null)
            {
                wrongTmTotal = wrong.TmIds.Count;
            }

            if (rightTotal > 0 && answerTotal > 0)
            {
                answerPercent = Math.Round((Convert.ToDouble(rightTotal) / answerTotal * 100), 0);
            }

            if (allAnswerTotal > 0 && allRightTotal > 0)
            {
                allPercent = Math.Round((Convert.ToDouble(allRightTotal) / allAnswerTotal * 100), 0);
            }

            if (collectRightTotal > 0 && collectAnswerTotal > 0)
            {
                collectPercent = Math.Round((Convert.ToDouble(collectRightTotal) / collectAnswerTotal * 100), 0);
            }


            if (wrongRightTotal > 0 && wrongAnswerTotal > 0)
            {
                wrongPercent = Math.Round((Convert.ToDouble(wrongRightTotal) / wrongAnswerTotal * 100), 0);
            }

            return (answerTotal, answerPercent, allTmTotal, allPercent, collectTmTotal, collectPercent, wrongTmTotal, wrongPercent);
        }
    }
}
