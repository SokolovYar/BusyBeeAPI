using BusyBee.Domain.Enums;
using BusyBee.Domain.Models;

namespace BusyBee.API.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<UserRole> UserRoles { get; set; } = null!;
    }
}
