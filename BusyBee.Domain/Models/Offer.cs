namespace BusyBee.Domain.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public DateTime PlannedDuration { get; set; }

        // Navigation properties
        public required Work Work { get; set; }
        public required Specialist Specialist { get; set; }
        public List<City> Cities { get; set; } = new List<City>();
    }
}
