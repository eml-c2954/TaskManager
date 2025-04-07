using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Data.Entities;
using TaskManager.Service.Dtos;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly System.Security.Claims.ClaimsPrincipal _currentUser;
        public TaskService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _currentUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<List<TaskDto>> GetTasksAsync(string? userId)
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            IEnumerable<EmlTask> tasks = [];
            if (currentUser.IsInRole("Admin"))
            {
                //Get all tasks
                tasks = await _dbContext.Tasks
                    .Include(x => x.Assignee)
                    .Where(x => string.IsNullOrEmpty(userId) || x.Assignee == null || x.Assignee.Id == userId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                throw new InvalidOperationException("User is not authorized to view tasks");
            }
            return tasks.Select(x => new TaskDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Status = x.Status,
                Assignee = x.Assignee != null ? new UserDto
                {
                    Email = x.Assignee?.Email ?? string.Empty,
                    UserName = x.Assignee?.UserName ?? string.Empty,
                } : null
            }).ToList();
        }

        public async Task<List<TaskDto>> GetPersonalTasks()
        {
            var userId = _userManager.GetUserId(_currentUser);
            var emlTasks = await _dbContext.Tasks
                    .Include(x => x.Assignee)
                    .Where(x => x.Assignee != null && x.Assignee.Id == userId)
                    .AsNoTracking()
                    .ToListAsync();

            return emlTasks.Select(x => new TaskDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Status = x.Status,
                Assignee = new UserDto
                {
                    Id = x.Assignee?.Id ?? string.Empty,
                    Email = x.Assignee?.Email ?? string.Empty,
                    UserName = x.Assignee?.UserName ?? string.Empty,
                }
            }).ToList();
        }
        public async Task<bool> CreateTaskAsync(TaskDto task)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var applicationUser = _userManager.GetUserAsync(user).Result;
            var taskEntity = new EmlTask
            {
                Title = task.Title,
                Description = task.Description,
                Status = Common.Enums.TaskStatusEnum.Pending,
                Assignee = applicationUser
            };
            _dbContext.Tasks.Add(taskEntity);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateTaskAsync(TaskDto task)
        {
            var taskEntity = _dbContext.Tasks.Include(x => x.Assignee).FirstOrDefault(x => x.Id == task.Id);
            if (taskEntity == null)
            {
                throw new InvalidOperationException("Task not found");
            }
            taskEntity.Title = task.Title;
            taskEntity.Description = task.Description;
            taskEntity.Status = task.Status;
            if (task.Assignee != null)
            {
                var user = await _userManager.FindByIdAsync(task.Assignee.Id);
                taskEntity.Assignee = user;
            }
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            EmlTask? task = _dbContext.Tasks.Find(id);

            if (task == null)
            {
                throw new InvalidOperationException("Task not found");
            }

            _dbContext.Tasks.Remove(task);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<TaskDto> GetTaskDetailsAsync(Guid id)
        {
            var task = await _dbContext.Tasks.Include(x => x.Assignee).FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                throw new InvalidOperationException("Task not found");
            }

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Assignee = new UserDto
                {
                    Id = task.Assignee?.Id ?? string.Empty,
                    Email = task.Assignee?.Email ?? string.Empty,
                    UserName = task.Assignee?.UserName ?? string.Empty
                }
            };
        }
    }
}
