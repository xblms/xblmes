﻿using Datory;
using XBLMS.Configuration;
using XBLMS.Core.Services;
using XBLMS.Utils;

namespace XBLMS.Core.Utils
{
    public static class InstallUtils
    {
        public static void SaveSettings(string contentRootPath, bool isProtectData, bool isSafeMode, bool isDisablePlugins, string securityKey, string databaseType, string databaseConnectionString, string redisConnectionString, string adminRestrictionHost, string[] adminRestrictionAllowList, string[] adminRestrictionBlockList, bool corsIsOrigins, string[] corsOrigins)
        {
            var path = PathUtils.Combine(contentRootPath, Constants.ConfigFileName);

            if (adminRestrictionAllowList == null)
            {
                adminRestrictionAllowList = new string[] { };
            }
            if (adminRestrictionBlockList == null)
            {
                adminRestrictionBlockList = new string[] { };
            }
            if (string.IsNullOrEmpty(securityKey))
            {
                securityKey = StringUtils.GetSecurityKey();
            }
            if (corsOrigins == null)
            {
                corsOrigins = new string[] { };
            }

            var json = SettingsManager.RunningInContainer
                ? $@"
{{
  ""IsDisablePlugins"": {StringUtils.ToLower(isDisablePlugins.ToString())},
  ""AdminRestriction"": {{
    ""Host"": ""{adminRestrictionHost}"",
    ""AllowList"": {TranslateUtils.JsonSerialize(adminRestrictionAllowList)},
    ""BlockList"": {TranslateUtils.JsonSerialize(adminRestrictionBlockList)}
  }},
  ""Cors"": {{
    ""IsOrigins"": {StringUtils.ToLower(corsIsOrigins.ToString())},
    ""Origins"": {TranslateUtils.JsonSerialize(corsOrigins)}
  }}
}}"
                : $@"
{{
  ""IsProtectData"": {StringUtils.ToLower(isProtectData.ToString())},
  ""IsSafeMode"": {StringUtils.ToLower(isSafeMode.ToString())},
  ""SecurityKey"": ""{securityKey}"",
  ""Database"": {{
    ""Type"": ""{databaseType}"",
    ""ConnectionString"": ""{StringUtils.ToJsonString(databaseConnectionString)}""
  }},
  ""Redis"": {{
    ""ConnectionString"": ""{redisConnectionString}""
  }},
  ""IsDisablePlugins"": {StringUtils.ToLower(isDisablePlugins.ToString())},
  ""AdminRestriction"": {{
    ""Host"": ""{adminRestrictionHost}"",
    ""AllowList"": {TranslateUtils.JsonSerialize(adminRestrictionAllowList)},
    ""BlockList"": {TranslateUtils.JsonSerialize(adminRestrictionBlockList)}
  }},
  ""Cors"": {{
    ""IsOrigins"": {StringUtils.ToLower(corsIsOrigins.ToString())},
    ""Origins"": {TranslateUtils.JsonSerialize(corsOrigins)}
  }}
}}";

            

            FileUtils.WriteText(path, json.Trim());

            var webConfigPath = PathUtils.Combine(contentRootPath, "Web.config");
            if (FileUtils.IsFileExists(webConfigPath))
            {
                var webConfigContent = FileUtils.ReadText(webConfigPath);
                FileUtils.WriteText(webConfigPath, webConfigContent);
            }
        }

        public static void Init(string contentRootPath)
        {
            if (SettingsManager.RunningInContainer)
            {
                var sourcePath = PathUtils.Combine(contentRootPath, "_wwwroot", DirectoryUtils.SiteFiles.DirectoryName, "version.txt");
                var sourceVersion = FileUtils.IsFileExists(sourcePath) ? FileUtils.ReadText(sourcePath) : string.Empty;
                var targetPath = PathUtils.Combine(contentRootPath, Constants.WwwrootDirectory, DirectoryUtils.SiteFiles.DirectoryName, "version.txt");
                var targetVersion = FileUtils.IsFileExists(targetPath) ? FileUtils.ReadText(targetPath) : string.Empty;
                if (sourceVersion != targetVersion)
                {
                    var directoryPath = PathUtils.Combine(contentRootPath, "_wwwroot");
                    foreach (var folderName in DirectoryUtils.GetDirectoryNames(directoryPath))
                    {
                        DirectoryUtils.Copy(
                            PathUtils.Combine(directoryPath, folderName),
                            PathUtils.Combine(contentRootPath, Constants.WwwrootDirectory, folderName),
                            true
                        );
                    }

                    foreach (var fileName in DirectoryUtils.GetFileNames(directoryPath))
                    {
                        if (fileName == "index.html") continue;
                        
                        FileUtils.CopyFile(
                            PathUtils.Combine(directoryPath, fileName),
                            PathUtils.Combine(contentRootPath, Constants.WwwrootDirectory, fileName),
                            false
                        );
                    }

                    FileUtils.WriteText(targetPath, sourceVersion);
                }
            }
            else
            {
                var wwwrootDirectory = PathUtils.Combine(contentRootPath, "wwwroot");
                DirectoryUtils.CreateDirectoryIfNotExists(wwwrootDirectory);
                
                var filePath = PathUtils.Combine(contentRootPath, Constants.ConfigFileName);
                if (FileUtils.IsFileExists(filePath))
                {
                    var json = FileUtils.ReadText(filePath);
                    if (json.Contains(@"""SecurityKey"": """","))
                    {
                        var securityKey = StringUtils.GetSecurityKey();
                        FileUtils.WriteText(filePath,
                            json.Replace(@"""SecurityKey"": """",", $@"""SecurityKey"": ""{securityKey}"","));
                    }
                }
                else
                {
                    var securityKey = StringUtils.GetSecurityKey();

                    SaveSettings(contentRootPath, false, false, false, securityKey, DatabaseType.MySql.GetValue(),
                        string.Empty, string.Empty, string.Empty, null, null, false, null);
                }
            }
        }

        public static string GetRedisConnectionString(string redisHost, bool isRedisDefaultPort, int redisPort, bool isSsl, string redisPassword)
        {
            var connectionString = string.Empty;
            if (!string.IsNullOrEmpty(redisHost))
            {
                var port = 6379;
                if (!isRedisDefaultPort && redisPort > 0)
                {
                    port = redisPort;
                }
                connectionString = $"{redisHost}:{port},allowAdmin=true";
                if (isSsl)
                {
                    connectionString += ",ssl=true";
                }
                if (!string.IsNullOrEmpty(redisPassword))
                {
                    connectionString += $",password={redisPassword}";
                }
            }

            return connectionString;
        }
    }
}
