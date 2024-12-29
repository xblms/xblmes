﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Utils;

namespace XBLMS.Web.Controllers.Home
{
    public partial class IndexController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            var config = await _configRepository.GetAsync();
            if (config.IsHomeClosed) return this.Error("对不起，用户中心已被禁用！");

            var user = await _authManager.GetUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var userMenus = await _userMenuRepository.GetUserMenusAsync();

            if (userMenus == null || userMenus.Count == 0)
            {
                await _userMenuRepository.ResetAsync();
                userMenus = await _userMenuRepository.GetUserMenusAsync();
            }

            var menus = new List<Menu>();

            foreach (var menuInfo1 in userMenus)
            {
                if (menuInfo1.Disabled || menuInfo1.ParentId > 0) continue;

                var children = new List<Menu>();
                foreach (var menuInfo2 in userMenus)
                {
                    if (menuInfo2.Disabled || menuInfo2.ParentId != menuInfo1.Id) continue;

                    children.Add(new Menu
                    {
                        Id = menuInfo2.Id.ToString(),
                        Text = menuInfo2.Text,
                        IconClass = menuInfo2.IconClass,
                        Link = menuInfo2.Link,
                        Target = menuInfo2.Target,
                        Name = menuInfo2.Name,

                    });
                }


                menus.Add(new Menu
                {
                    Id = menuInfo1.Id.ToString(),
                    Text = menuInfo1.Text,
                    IconClass = menuInfo1.IconClass,
                    Link = menuInfo1.Link,
                    Target = menuInfo1.Target,
                    Children = children,
                    Name = menuInfo1.Name,
                });
            }

            return new GetResult
            {
                User = user,
                Menus = menus,
                Version = _settingsManager.Version
            };
        }
    }
}
