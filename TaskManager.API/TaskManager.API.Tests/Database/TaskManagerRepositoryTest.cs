using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Database;
using TaskManager.API.Models;
using TaskManager.API.Tests.Database.Helper;
using Xunit;

namespace TaskManager.API.Tests.Database
{
    public class TaskManagerRepositoryTest : IDisposable
    {
        public TaskManagerRepositoryTest()
        {
        }

        [Fact]
        public async Task TestGetAll_ReturnsTwoTaskDetails()
        {
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskManagerRepository(mockContext.Object);

            IQueryable<TaskItem> taskDetailsList = new List<TaskItem>()
            {
                new TaskItem() {Id = 1, Name ="Task 1 ", Priority = 10},
                new TaskItem() {Id = 2, Name ="Task 2 ", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TaskItem>>();

            mockSet.As<IAsyncEnumerable<TaskItem>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<TaskItem>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<TaskItem>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TaskItem>(taskDetailsList.Provider));

            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            var taskDetails = await taskRepository.GetAllAsync();

            Assert.Equal(2, taskDetails.Count());
        }

        [Fact]
        public async Task TestGet_VerifyTaskName()
        {

            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskManagerRepository(mockContext.Object);

            IQueryable<TaskItem> taskDetailsList = new List<TaskItem>()
            {
                new TaskItem() {Id = 1, Name ="Task 1", Priority = 10},
                new TaskItem() {Id = 2, Name ="Task 2", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TaskItem>>();

            mockSet.As<IAsyncEnumerable<TaskItem>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<TaskItem>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<TaskItem>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<TaskItem>(taskDetailsList.Provider));

            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            var taskDetails = await taskRepository.GetAsync(2);

            Assert.Equal("Task 2", taskDetails.Name);
        }

        [Fact]
        public async Task TestInsertAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskManagerRepository(mockContext.Object);

            var taskDetail = new TaskItem() { Id = 1, Name = "Task 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<TaskItem>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            var result = await taskRepository.InsertAsync(taskDetail);

            mockSet.Verify(m => m.Add(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task TestUpdateAsync_VerifySaveChangesCalledOnce()
        {
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskManagerRepository(mockContext.Object);

            var taskDetail = new TaskItem() { Id = 1, Name = "Task 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<TaskItem>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);
            await taskRepository.UpdateAsync(1, taskDetail);

            mockSet.Verify(m => m.Update(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        public void Dispose()
        {
        }
    }
}
