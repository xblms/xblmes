﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin
{
    public partial class SyncDatabaseController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            if (await _configRepository.IsNeedInstallAsync())
            {
                return this.Error("系统未安装，向导被禁用！");
            }

            var config = await _configRepository.GetAsync();

            return new GetResult
            {
                DatabaseVersion = config.DatabaseVersion,
                Version = _settingsManager.Version
            };
        }
    }
}
