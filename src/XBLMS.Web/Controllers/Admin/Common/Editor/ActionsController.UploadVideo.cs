﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XBLMS.Configuration;
using XBLMS.Dto;
using XBLMS.Enums;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Admin.Common.Editor
{
    public partial class ActionsController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(RouteActionsUploadVideo)]
        public async Task<ActionResult<UploadVideoResult>> UploadVideo([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return new UploadVideoResult
                {
                    Error = Constants.ErrorUpload
                };
            }

            var original = Path.GetFileName(file.FileName);
            var fileName = _pathManager.GetUploadFileName(original);

            if (!_pathManager.IsVideoExtensionAllowed(PathUtils.GetExtension(fileName)))
            {
                return new UploadVideoResult
                {
                    Error = Constants.ErrorVideoExtensionAllowed
                };
            }
            if (!_pathManager.IsVideoSizeAllowed(file.Length))
            {
                return new UploadVideoResult
                {
                    Error = Constants.ErrorVideoSizeAllowed
                };
            }

            var filePath =  _pathManager.GetEditUploadFilesPath(fileName);
            await _pathManager.UploadAsync(file, filePath);

            var videoUrl =  _pathManager.GetRootUrlByPath(filePath);

            return new UploadVideoResult
            {
                State = "SUCCESS",
                Url = videoUrl,
                Title = original,
                Original = original,
                Error = null
            };
        }
    }
}
