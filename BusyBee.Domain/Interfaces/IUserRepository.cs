﻿
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
    }
}
