using TaskManager.Service.Dtos;

namespace TaskManager.Service.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetTasksAsync(string? userId);
        Task<List<TaskDto>> GetPersonalTasks();
        Task<TaskDto> GetTaskDetailsAsync(Guid id);
        Task<bool> CreateTaskAsync(TaskDto task);
        Task<bool> UpdateTaskAsync(TaskDto task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
