using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Database
{
    public class TaskManagerRepository : ITaskManagerRepository
    {
        private readonly TaskManagerDbContext taskManagerDbContext;

        public TaskManagerRepository(TaskManagerDbContext taskManagerDbContext)
        {
            this.taskManagerDbContext = taskManagerDbContext;
        }

        public async Task DeleteAsync(TaskItem entity)
        {
            this.taskManagerDbContext.Tasks.Remove(entity);

            await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await this.taskManagerDbContext.Tasks.AsNoTracking<TaskItem>().ToListAsync();
        }

        public async Task<TaskItem> GetAsync(int id)
        {
            return await taskManagerDbContext.Tasks.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<int> InsertAsync(TaskItem entity)
        {
            taskManagerDbContext.Tasks.Add(entity);

            return await taskManagerDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, TaskItem entity)
        {
            taskManagerDbContext.Tasks.Update(entity);

            await taskManagerDbContext.SaveChangesAsync();
        }
    }
}
