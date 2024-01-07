using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorToDoWebAppSample.Components.Models;
using BlazorToDoWebAppSample.Components.Data;

namespace BlazorToDoWebAppSample.Components.Controllers
{
    public class TodoTasksController : Controller
    {
        private readonly IDbContextFactory<BlazorToDoWebAppSampleDbContext> _contextFactory;
        private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

        public TodoTasksController(IDbContextFactory<BlazorToDoWebAppSampleDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<TodoTask>> GetAllTodoTasksAsync()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    return await context.TodoTask.ToListAsync();
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task<TodoTask?> FindTodoTasksAsync(int id)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    return await context.TodoTask.FindAsync(id);
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task AddTodoTaskAsync(TodoTask todoTask)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    context.TodoTask.Add(todoTask);
                    await context.SaveChangesAsync();
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task UpdateTodoTaskAsync(TodoTask todoTask)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    if (context.TodoTask.Any(e => e.Id == todoTask.Id))
                    {
                        await context.Database.EnsureCreatedAsync();
                        context.TodoTask.Update(todoTask);
                        await context.SaveChangesAsync();
                    }
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task DeleteTodoTaskAsync(int id)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var todoTask = await context.TodoTask.FindAsync(id);
                    if (todoTask != null)
                    {
                        await context.Database.EnsureCreatedAsync();
                        context.TodoTask.Remove(todoTask);
                        await context.SaveChangesAsync();
                    }
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task DeleteAllTodoTasksAsync()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                    context.TodoTask.RemoveRange(context.TodoTask);
                    await context.SaveChangesAsync();
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
