using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusyBee.Domain.Models
{
    public class Work
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public WorkCategory WorkCategory { get; set; } = null!;
    }
}
