using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusyBee.Domain.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Range(1, 5)] 
        public int Rating { get; set; } // Assuming rating is an integer value
        // Navigation properties
        public required Work Work { get; set; }
        public required Specialist Specialist { get; set; }

    }
}
