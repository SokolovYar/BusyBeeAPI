using Microsoft.AspNetCore.Identity;

namespace BusyBee.Domain.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
