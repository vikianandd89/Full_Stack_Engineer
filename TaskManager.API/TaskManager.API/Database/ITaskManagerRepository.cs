using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Database
{
    public interface ITaskManagerRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();

        Task<TaskItem> GetAsync(int id);

        Task<int> InsertAsync(TaskItem entity);

        Task UpdateAsync(int id, TaskItem entity);

        Task DeleteAsync(TaskItem entity);
    }
}
