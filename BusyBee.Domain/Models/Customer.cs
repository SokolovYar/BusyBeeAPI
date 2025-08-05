using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusyBee.Domain.Models
{
    public class Customer
    {
        public int Id { get; set; }
        //public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime LastActivityTime { get; set; }
        public bool IsOnline => DateTime.UtcNow - LastActivityTime < TimeSpan.FromMinutes(5);
    }
}
