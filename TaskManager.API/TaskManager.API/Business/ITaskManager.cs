using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Business
{
    public interface ITaskManager
    {
        Task<int> AddTaskAsync(TaskItem taskItem);

        Task<IEnumerable<TaskItem>> GetAllTasksAsync();

        Task<TaskItem> GetTaskAsync(int id);

        Task UpdateTaskAsync(int id, TaskItem taskItem);

        bool IsTaskItemValid(TaskItem taskItem);
    }
}
