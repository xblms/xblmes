using Datory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XBLMS.Core.Utils;
using XBLMS.Models;
using XBLMS.Repositories;
using XBLMS.Services;

namespace XBLMS.Core.Repositories
{
    public class ExamCerRepository : IExamCerRepository
    {
        private readonly Repository<ExamCer> _repository;
        private readonly string _cacheKey;

        public ExamCerRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<ExamCer>(settingsManager.Database, settingsManager.Redis);
            _cacheKey = CacheUtils.GetEntityKey(TableName);
        }

        public IDatabase Database => _repository.Database;

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;
        public async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public async Task<ExamCer> GetAsync(int id)
        {
            var list = await GetListAsync();
            return list.FirstOrDefault(cer => cer.Id == id) ?? null;
        }

        public async Task<List<ExamCer>> GetListAsync()
        {
            var list = (await _repository.GetAllAsync(Q
                .OrderBy(nameof(ExamCer.Id))
                .CachingGet(_cacheKey)
            )).ToList();
            return list;
        }

        public async Task<int> InsertAsync(ExamCer item)
        {
            return await _repository.InsertAsync(item, Q.CachingRemove(_cacheKey));
        }

        public async Task UpdateAsync(ExamCer item)
        {
            await _repository.UpdateAsync(item, Q.CachingRemove(_cacheKey));
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id, Q.CachingRemove(_cacheKey));
        }

        public async Task<(int allCount, int addCount, int deleteCount, int lockedCount, int unLockedCount)> GetDataCount()
        {
            var count = await _repository.CountAsync();
            return (count, 0, 0, 0, count);
        }
    }
}
