using Microsoft.EntityFrameworkCore;
using BusyBee.Domain.Models;

namespace BusyBee.DataAccess
{
    public class BusyBeeDBContext : DbContext
    {
        public BusyBeeDBContext(DbContextOptions<BusyBeeDBContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Specialist> Specialists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<WorkCategory> WorkCategories { get; set; }
    }
}
