using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Dto;
using XBLMS.Core.Utils;
using XBLMS.Utils;
using XBLMS.Enums;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTmTreeController
    {
        [HttpPost, Route(RouteDelete)]
        public async Task<ActionResult<BoolResult>> Delete([FromBody] IdRequest request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Delete))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();

            var item = await _examTmTreeRepository.GetAsync(request.Id);

            if (item == null) return this.NotFound();
            var ids = await _examTmTreeRepository.GetIdsAsync(request.Id);
            var tmCount =0;
            if (tmCount > 0) return this.Error($"�÷������������{tmCount}����Ŀ����ʱ������ɾ��");
            await _examTmTreeRepository.DeleteAsync(ids);
            await _authManager.AddAdminLogAsync("ɾ����Ŀ���༰�����¼�", $"�������ƣ�{item.Name}");
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
