using Datory;
using System;
using System.Threading.Tasks;

namespace XBLMS.Repositories
{
    public interface IDbCacheRepository : IRepository
    {
        Task RemoveAndInsertAsync(string cacheKey, string cacheValue);

        Task RemoveAsync(string cacheKey);

        Task ClearAsync();

        Task ClearAllExceptAdminSessionsAsync();

        Task<string> GetValueAndRemoveAsync(string cacheKey);

        Task<string> GetValueAsync(string cacheKey);

        Task<(string, DateTime)> GetValueAndCreatedDateAsync(string cacheKey);
    }
}