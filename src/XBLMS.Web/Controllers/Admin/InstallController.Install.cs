﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class InstallController
    {
        [HttpPost, Route(RouteInstall)]
        public async Task<ActionResult<BoolResult>> Install([FromBody]InstallRequest request)
        {
            if (request.SecurityKey != _settingsManager.SecurityKey) return Unauthorized();

            var (success, errorMessage) = await _databaseManager.InstallAsync(request.CompanyName,request.UserName, request.AdminPassword, request.Email, request.Mobile);
            if (!success)
            {
                return this.Error(errorMessage);
            }

            await FileUtils.WriteTextAsync(_pathManager.GetRootPath("index.html"), Constants.Html5Empty);

            return new BoolResult
            {
                Value = true
            };
        }
    }
}
