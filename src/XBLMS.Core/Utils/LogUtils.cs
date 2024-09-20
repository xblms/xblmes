﻿using System;
using System.Collections.Generic;

namespace XBLMS.Core.Utils
{
    public static class LogUtils
    {
        public const string CategoryAdmin = "admin";
        public const string CategoryHome = "home";
        public const string CategoryApi = "api";

        public static readonly Lazy<List<KeyValuePair<string, string>>> AllCategoryList = new Lazy<List<KeyValuePair<string, string>>>(
            () =>
            {
                var list = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(CategoryAdmin, "后台错误"),
                    new KeyValuePair<string, string>(CategoryHome, "用户中心错误"),
                    new KeyValuePair<string, string>(CategoryApi, "API错误")
                };
                return list;
            });
    }
}
