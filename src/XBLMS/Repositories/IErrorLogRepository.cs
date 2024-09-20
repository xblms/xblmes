using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public partial interface IErrorLogRepository : IRepository
    {
        Task<int> InsertAsync(ErrorLog logInfo);

        Task DeleteIfThresholdAsync();

        Task DeleteAllAsync();

        Task<ErrorLog> GetErrorLogAsync(int logId);

        Task<int> GetCountAsync(string category, string keyword, string dateFrom, string dateTo);

        Task<List<ErrorLog>> GetAllAsync(string category, string keyword, string dateFrom,
            string dateTo, int offset, int limit);
    }
}
