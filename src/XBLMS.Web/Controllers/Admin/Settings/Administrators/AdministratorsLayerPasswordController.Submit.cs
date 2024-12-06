﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Settings.Administrators
{
    public partial class AdministratorsLayerPasswordController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<BoolResult>> Submit([FromBody] SubmitRequest request)
        {
            var userName = request.UserName;
            var adminName = _authManager.AdminName;
            var password = StringUtils.Base64Decode(request.Password);

            if (string.IsNullOrEmpty(userName)) userName = adminName;
            var adminInfo = await _administratorRepository.GetByUserNameAsync(userName);
            if (adminInfo == null) return this.Error(Constants.ErrorNotFound);

            if (request.UserName == _authManager.AdminName)
            {
                var oldPassword = StringUtils.Base64Decode(request.OldPassword);
                var (administrator, _, _) = await _administratorRepository.ValidateAsync(userName, oldPassword, false);
                if (administrator == null)
                {
                    return this.Error($"更改密码失败：旧密码不正确");
                }
                if (oldPassword == password)
                {
                    return this.Error($"更改密码失败：新密码不能和旧密码相同");
                }
            }

            var (isValid, errorMessage) = await _administratorRepository.ChangePasswordAsync(adminInfo, password);
            if (!isValid)
            {
                return this.Error($"更改密码失败：{errorMessage}");
            }

            await _authManager.AddAdminLogAsync("重设管理员密码", $"{adminInfo.UserName}");
            await _authManager.AddStatLogAsync(StatType.AdminUpdate, "重设管理员密码", adminInfo.Id, adminInfo.UserName);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
