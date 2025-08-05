using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusyBee.Domain.Models
{
    public enum OrderStatus
    {
        Pending,    
        InProgress,  
        Completed,   
        Cancelled    
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;


        // Navigation properties
        public required Work Work { get; set; }
        public required Customer Customer { get; set; }
        public required Specialist Specialist { get; set; }
        public User? EditedBy { get; set; }
    }
 }
