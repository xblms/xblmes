using Datory;
using System.Collections.Generic;
using System.Threading.Tasks;
using XBLMS.Models;

namespace XBLMS.Repositories
{
    public interface IExamPkRepository : IRepository
    {
        Task<ExamPk> GetAsync(int id);
        Task<ExamPk> GetNextAsync(int parentId, int current);
        Task<ExamPk> GetNextAsync(int parentId, int current, int vs);
        Task<bool> IsExistsAsync(string name);
        Task<int> InsertAsync(ExamPk examPk);
        Task UpdateAsync(ExamPk examPk);
        Task DeleteAsync(int id);
        Task<List<ExamPk>> GetChildList(int id);
        Task<(int total, List<ExamPk> list)> GetListAsync(string name, int pageIndex, int pageSize);
    }
}