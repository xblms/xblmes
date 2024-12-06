using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Models;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Exam
{
    public partial class ExamTxEditController
    {
        [HttpPost, Route(RouteAdd)]
        public async Task<ActionResult<BoolResult>> Add([FromBody] ItemRequest<ExamTx> request)
        {
            if (!await _authManager.HasPermissionsAsync(MenuPermissionType.Add))
            {
                return this.NoAuth();
            }

            var admin = await _authManager.GetAdminAsync();
            var tx = request.Item;
            if (await _examTxRepository.IsExistsAsync(tx.Name))
            {
                return this.Error("����ʧ�ܣ��Ѵ�����ͬ���Ƶ����ͣ�");
            }
            tx.CompanyId = admin.CompanyId;
            tx.DepartmentId = admin.DepartmentId;
            tx.CreatorId = admin.Id;

            var txId = await _examTxRepository.InsertAsync(tx);
            await _authManager.AddAdminLogAsync("��������", $"{tx.Name}");
            await _authManager.AddStatLogAsync(StatType.ExamTxAdd, "��������", txId, tx.Name);
            await _authManager.AddStatCount(StatType.ExamTxAdd);
            return new BoolResult
            {
                Value = true
            };
        }
    }
}
