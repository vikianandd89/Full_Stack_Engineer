using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Database;
using TaskManager.API.Models;

namespace TaskManager.API.Business
{
    public class TaskManager : ITaskManager
    {
        private readonly ITaskManagerRepository taskManagerRepository;
        private readonly ILogger<TaskManager> logger;

        public TaskManager(
            ITaskManagerRepository taskManagerRepository,
            ILogger<TaskManager> logger)
        {
            this.taskManagerRepository = taskManagerRepository;
            this.logger = logger;
        }

        public async Task<int> AddTaskAsync(TaskItem taskItem)
        {
            return await taskManagerRepository.InsertAsync(taskItem);
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await taskManagerRepository.GetAllAsync();
        }

        public async Task<TaskItem> GetTaskAsync(int id)
        {
            return await taskManagerRepository.GetAsync(id);
        }

        public async Task UpdateTaskAsync(int id, TaskItem taskItem)
        {
            await this.taskManagerRepository.UpdateAsync(id, taskItem);
        }

        public bool IsTaskItemValid(TaskItem taskItem)
        {
            this.logger.LogInformation($"checking task is valid to close or not");
            var taskItems = this.taskManagerRepository.GetAllAsync().Result;
            var isValid = !taskItems.Any(t => t.ParentTaskId == taskItem.Id && t.EndTask == false);

            var logMessage = (isValid) 
                            ? $"Task {taskItem.Name} is valid to close"
                            : $"Task {taskItem.Name} is not valid to close";

            this.logger.LogInformation(logMessage);

            return isValid;
        }
    }
}
