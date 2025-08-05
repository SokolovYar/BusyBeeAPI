using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusyBee.Domain.Models
{
    public class Specialist
    {
        public static int TIME_TO_ONLINE = 5;
        public int Id { get; set; }
       // public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime LastActivityTime { get; set; }
        public bool IsOnline => DateTime.UtcNow - LastActivityTime < TimeSpan.FromMinutes(TIME_TO_ONLINE);
    }
}
