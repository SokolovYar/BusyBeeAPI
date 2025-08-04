using BusyBee.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusyBee.DataAccess
{
    public class BusyBeeDBContext : IdentityDbContext<User>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Offer>()
                .HasMany(o => o.Cities)
                .WithMany(c => c.Offers)
                .UsingEntity(j => j.ToTable("OfferCities"));
        }

    }
}
