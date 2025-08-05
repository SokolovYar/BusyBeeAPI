using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusyBee.Domain.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "UAH";
        public string Status { get; set; } = "Pending"; // или enum
        public string PaymentProvider { get; set; } = "LiqPay";
        public string ExternalPaymentId { get; set; } // ID в платёжной системе
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }


        public required Customer Customer { get; set; } 
        public required Order Order { get; set; } 
        


    }
}
