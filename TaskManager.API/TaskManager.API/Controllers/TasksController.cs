using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.API.Business;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Tasks")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly ITaskManager taskManager;
        private readonly ILogger<TasksController> logger;

        public TasksController(
            ITaskManager taskManager,
            ILogger<TasksController> logger)
        {
            this.taskManager = taskManager;
            this.logger = logger;
        }

        // GET api/Tasks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                this.logger.LogInformation("Entering into Get all tasks method");
                return Ok(await taskManager.GetAllTasksAsync());
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Please try again later");
            }
        }

        // GET api/Tasks/1
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                this.logger.LogInformation($"Getting task detail for {id}");
                return Ok(await taskManager.GetTaskAsync(id));
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Issue with server, please try again later");
            }
        }

        // POST api/Tasks
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskItem taskItem)
        {
            try
            {
                if(taskItem == null)
                {
                    this.logger.LogInformation("Provide valid task item detail");
                    return BadRequest();
                }

                await taskManager.AddTaskAsync(taskItem);
                this.logger.LogInformation($"Task {taskItem.Id} created successfully");

                return Ok($"Task {taskItem.Id} created successfully");
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Please try again later");
            }
        }

        // PUT api/Tasks/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TaskItem taskItem)
        {
            try
            {
                if (taskItem == null || taskItem.Id != id)
                {
                    this.logger.LogInformation("Provide valid task item detail");
                    return BadRequest("Provide a valid task");
                }

                if (!taskManager.IsTaskItemValid(taskItem))
                {
                    this.logger.LogInformation("You can not close this task as it has child tasks");
                    return BadRequest("You can not close this task as it has child tasks");
                }

                await taskManager.UpdateTaskAsync(id, taskItem);
                this.logger.LogInformation($"Task {taskItem.Name} updated successfully");

                return Ok($"Task {taskItem.Name} updated successfully");
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Please try again later");
            }
        }
    }
}
