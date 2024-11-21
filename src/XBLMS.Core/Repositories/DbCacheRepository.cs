using Datory;
using Datory.Caching;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Configuration;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class DbCacheRepository : IDbCacheRepository
    {
        private readonly Repository<DbCache> _repository;
        private readonly ISettingsManager _settingsManager;
        private readonly ICacheManager _cacheManager;

        public DbCacheRepository(ISettingsManager settingsManager, ICacheManager cacheManager)
        {
            _settingsManager = settingsManager;
            _cacheManager = cacheManager;
            _repository = new Repository<DbCache>(settingsManager.Database, settingsManager.Redis);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        public async Task RemoveAndInsertAsync(string cacheKey, string cacheValue)
        {
            if (string.IsNullOrEmpty(cacheKey)) return;

            await _repository.DeleteAsync(Q
                .Where(nameof(DbCache.CacheKey), cacheKey)
                .CachingRemove(cacheKey)
            );

            await _repository.InsertAsync(new DbCache
            {
                CacheKey = cacheKey,
                CacheValue = cacheValue
            });
        }

        public async Task RemoveAsync(string cacheKey)
        {
            if (string.IsNullOrEmpty(cacheKey)) return;

            await _repository.DeleteAsync(Q
                .Where(nameof(DbCache.CacheKey), cacheKey)
                .CachingRemove(cacheKey)
            );
        }

        public async Task ClearAsync()
        {
            var cacheKeys = await _repository.GetAllAsync<string>(Q
                .Select(nameof(DbCache.CacheKey))
            );
            await _repository.RemoveCacheAsync(cacheKeys.ToArray());
            await _repository.DeleteAsync();
        }

        public async Task ClearAllExceptAdminSessionsAsync()
        {
            var dbCaches = await _repository.GetAllAsync(Q
                .WhereLike(nameof(DbCache.CacheKey), $"{Constants.SessionIdPrefix}%")
            );

            await ClearAsync();
            var cacheManager = await CachingUtils.GetCacheManagerAsync(_settingsManager.Redis);
            cacheManager.Clear();
            _cacheManager.Clear();

            foreach (var dbCache in dbCaches)
            {
                if (!string.IsNullOrEmpty(dbCache.CacheKey) && !string.IsNullOrEmpty(dbCache.CacheValue))
                {
                    await RemoveAndInsertAsync(dbCache.CacheKey, dbCache.CacheValue);
                }
            }
        }

        public async Task<string> GetValueAndRemoveAsync(string cacheKey)
        {
            var retVal = await _repository.GetAsync<string>(Q
                .Select(nameof(DbCache.CacheValue))
                .Where(nameof(DbCache.CacheKey), cacheKey)
            );

            await _repository.DeleteAsync(Q
                .Where(nameof(DbCache.CacheKey), cacheKey)
                .CachingRemove(cacheKey)
            );

            return retVal;
        }

        public async Task<string> GetValueAsync(string cacheKey)
        {
            var retVal = await _repository.GetAsync<string>(Q
                .Select(nameof(DbCache.CacheValue))
                .Where(nameof(DbCache.CacheKey), cacheKey)
                .CachingGet(cacheKey)
            );

            return retVal;
        }

        public async Task<(string, DateTime)> GetValueAndCreatedDateAsync(string cacheKey)
        {
            var retVal = await _repository.GetAsync<(string, DateTime)>(Q
                .Select(nameof(DbCache.CacheValue), nameof(DbCache.CreatedDate))
                .Where(nameof(DbCache.CacheKey), cacheKey)
            );

            return retVal;
        }
    }
}
