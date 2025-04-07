using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Service.Dtos;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<UserDto>> GetUsers()
        {
            var users = await _dbContext.Users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToListAsync();
            return users;
        }
    }
}
