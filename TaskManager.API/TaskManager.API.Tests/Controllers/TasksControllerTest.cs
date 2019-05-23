using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TaskManager.API.Business;
using TaskManager.API.Controllers;
using TaskManager.API.Models;
using Xunit;

namespace TaskManager.API.Tests.Controllers
{
    public class TasksControllerTest : IDisposable
    {
        public ILogger<TasksController> Logger { get; private set; }

        public TasksControllerTest()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<TasksController>();
        }

        [Fact]
        public async Task TestGetAllAsync_VerifyServiceReturnOkStatus()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);

            var taskDetailsList = new List<TaskItem>()
            {
                new TaskItem() {Id = 1, Name ="Task 1 ", Priority = 10},
                new TaskItem() {Id = 2, Name ="Task 2 ", Priority = 20},
            };

            mockManageTask.Setup(manage => manage.GetAllTasksAsync()).Returns(Task.FromResult<IEnumerable<TaskItem>>(taskDetailsList));

            var statusResult = await taskRepository.Get();

            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as List<TaskItem>;
            Assert.Equal(2, taskDetailsResult.Count);
        }

        [Fact]
        public async Task TestGetAllAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);

            mockManageTask.Setup(manage => manage.GetAllTasksAsync()).Throws(new Exception());

            var statusResult = await taskRepository.Get();

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }


        [Fact]
        public async Task TestGetAsync_VerifyServiceReturnOkStatusAndCheckTaskDetails()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);

            var taskDetail = new TaskItem() { Id = 1, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.GetTaskAsync(1)).Returns(Task.FromResult<TaskItem>(taskDetail));

            var statusResult = await taskRepository.Get(1);

            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as TaskItem;
            Assert.Equal("Task 1", taskDetailsResult.Name);
            Assert.Equal(10, taskDetailsResult.Priority);
        }


        [Fact]
        public async Task TestGetAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);

            mockManageTask.Setup(manage => manage.GetTaskAsync(1)).Throws(new Exception());

            var statusResult = await taskRepository.Get(1);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPostAsync_VerifyServiceReturnOkStatusAndCheckTaskId()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);

            var taskDetail = new TaskItem() { Id = 1001, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.AddTaskAsync(taskDetail)).Returns(Task.FromResult<int>(1001));

            var statusResult = await taskRepository.Post(taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task 1001 created successfully", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPostAsync_PassNullAndVerifyServiceReturnBadRequest()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);

            var statusResult = await taskRepository.Post(null);

            Assert.NotNull(statusResult as BadRequestResult);
        }

        [Fact]
        public async Task TestPostAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);
            var taskDetail = new TaskItem() { Id = 1001, Name = "Task 1", Priority = 10 };
            mockManageTask.Setup(manage => manage.AddTaskAsync(taskDetail)).Throws(new Exception());

            var statusResult = await taskRepository.Post(taskDetail);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusAndCheckServiceResponse()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);
            var taskDetail = new TaskItem() { Id = 1001, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.IsTaskItemValid(taskDetail)).Returns(true);
            mockManageTask.Setup(manage => manage.UpdateTaskAsync(1001, taskDetail)).Returns(Task.FromResult<int>(1001));

            var statusResult = await taskRepository.Put(1001, taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task Task 1 updated successfully", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailNull()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);

            var statusResult = await taskRepository.Put(1001, null);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Provide a valid task", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailIdIsInvalid()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);
            var taskDetail = new TaskItem() { Id = 1001, Name = "Task 1", Priority = 10 };
            var statusResult = await taskRepository.Put(1002, taskDetail);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Provide a valid task", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnBadRequestWhenTaskDetailIsNotValidToClose()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);
            var taskDetail = new TaskItem() { Id = 1001, Name = "Task 1", Priority = 10, EndTask = true };
            mockManageTask.Setup(manage => manage.IsTaskItemValid(taskDetail)).Returns(false);
            var statusResult = await taskRepository.Put(1001, taskDetail);

            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("You can not close this task as it has child tasks", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_VerifyServiceReturnOkStatusWhenTaskDetailIsValidToClose()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);

            var taskDetail = new TaskItem() { Id = 1001, Name = "Task 1", Priority = 10, EndTask = true };

            mockManageTask.Setup(manage => manage.IsTaskItemValid(taskDetail)).Returns(true);

            mockManageTask.Setup(manage => manage.UpdateTaskAsync(1001, taskDetail)).Returns(Task.FromResult<int>(1001));

            var statusResult = await taskRepository.Put(1001, taskDetail);

            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task Task 1 updated successfully", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task TestPutAsync_WhenManageTaskThrowsExceptionVerifyServiceReturnInternalServerErrorStatus()
        {
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TasksController(mockManageTask.Object, this.Logger);
            var taskDetail = new TaskItem() { Id = 1001, Name = "Task 1", Priority = 10 };
            mockManageTask.Setup(manage => manage.IsTaskItemValid(taskDetail)).Returns(true);
            mockManageTask.Setup(manage => manage.UpdateTaskAsync(1001, taskDetail)).Throws(new Exception());

            var statusResult = await taskRepository.Put(1001, taskDetail);

            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult).StatusCode);
        }

        public void Dispose()
        {
        }
    }
}
