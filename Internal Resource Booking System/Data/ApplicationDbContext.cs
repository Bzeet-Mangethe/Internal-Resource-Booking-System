using InternalResourceBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternalResourceBookingSystem.Data
{
   // public class ApplicationDbContext : DbContext
public class ApplicationDbContext : IdentityDbContext<IdentityUser>

    // public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        //Below to be removed if there is an error
        public DbSet<IdentityUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>()
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Resource)
                .HasForeignKey(b => b.ResourceId);

            base.OnModelCreating(modelBuilder);
        }
    }
}