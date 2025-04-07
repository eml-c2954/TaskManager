using TaskManager.Service.Dtos;

namespace TaskManager.Service.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsers();
    }
}
