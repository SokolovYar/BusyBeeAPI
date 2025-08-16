
using BusyBee.Domain.Enums;
using BusyBee.Domain.Models;

namespace BusyBee.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task DeleteUserAsync(string userId);
        Task<User> GetUserByIdAsync(string userId);
        Task<User?> GetRequiredUserAsync(string email);
        Task UpdateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task ChangeUserRoleAsync(string userId, string newUserRole);
        Task<List<UserRole>> GetUserRolesAsync(string userId);

    }
}
