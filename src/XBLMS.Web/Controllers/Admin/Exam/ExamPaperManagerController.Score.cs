using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamPaperManagerController
    {
        [HttpGet, Route(RouteScore)]
        public async Task<ActionResult<GetScoreResult>> GetSocreList([FromQuery] GetSocreRequest request)
        {
            var (total, list) = await _examPaperStartRepository.GetListByAdminAsync(request.Id, request.DateFrom, request.DateTo, request.Keywords, request.PageIndex, request.PageSize);
            if (total > 0)
            {
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);

                    user.Set("UseTime", DateUtils.SecondToHms(item.ExamTimeSeconds));

                    item.Set("User", user);
                }
            }
            return new GetScoreResult
            {
                Total = total,
                List = list
            };
        }

        [HttpPost, Route(RouteScoreExport)]
        public async Task<ActionResult<StringResult>> SocreExport([FromBody] GetSocreRequest request)
        {
            var (total, list) = await _examPaperStartRepository.GetListByAdminAsync(request.Id, request.DateFrom, request.DateTo, request.Keywords, 1, int.MaxValue);

            var paper = await _examPaperRepository.GetAsync(request.Id);

            var fileName = $"{paper.Title}-�ɼ���.xlsx";
            var filePath = _pathManager.GetDownloadFilesPath(fileName);

            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            var head = new List<string>
            {
                "���",
                "�˺�",
                "����",
                "��֯",
                "����ʱ��",
                "����ʱ��",
                "��ʱ",
                "�͹���ɼ�",
                "������ɼ�",
                "�ɼ�",
            };
            var rows = new List<List<string>>();


            if (total > 0)
            {
                var index = 1;
                foreach (var item in list)
                {
                    var user = await _organManager.GetUser(item.UserId);

                    rows.Add(new List<string>
                    {
                        index.ToString(),
                        user.UserName,
                        user.DisplayName,
                        user.Get("OrganNames").ToString(),
                        $"{item.BeginDateTime.Value}",
                        $"{item.EndDateTime.Value}",
                        DateUtils.SecondToHms(item.ExamTimeSeconds),
                        $"{item.ObjectiveScore}",
                        $"{item.SubjectiveScore}",
                        $"{item.Score}"
                    });
                    index++;

                }
            }

            ExcelUtils.Write(filePath, head, rows);

            var downloadUrl = _pathManager.GetDownloadFilesUrl(fileName);

            return new StringResult
            {
                Value = downloadUrl
            };
        }
    }
}