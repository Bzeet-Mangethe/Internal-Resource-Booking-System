using Microsoft.AspNetCore.Identity;
using InternalResourceBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalResourceBookingSystem.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed roles and admin user
            await SeedIdentityAsync(userManager, roleManager);

            // Seed resources and bookings
            SeedResources(context);
        }

        private static async Task SeedIdentityAsync(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Seed roles
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed admin user
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private static void SeedResources(ApplicationDbContext context)
        {
            // Skip seeding if already exists
            if (context.Resources.Any()) return;

            var resources = new List<Resource>
            {
                new Resource
                {
                    Name = "Meeting Room Alpha",
                    Description = "Large room with projector and whiteboard",
                    Location = "3rd Floor, West Wing",
                    Capacity = 10,
                    IsAvailable = true
                },
                new Resource
                {
                    Name = "Company Car 1",
                    Description = "Compact sedan for business use",
                    Location = "Parking Bay 5",
                    Capacity = 4,
                    IsAvailable = true
                }
            };

            context.Resources.AddRange(resources);
            context.SaveChanges();

            var bookings = new List<Booking>
            {
                new Booking
                {
                    ResourceId = resources[0].Id,
                    StartTime = DateTime.Today.AddHours(9),
                    EndTime = DateTime.Today.AddHours(10),
                    BookedBy = "Bhekani Masinga",
                    Purpose = "Team Stand-up"
                },
                new Booking
                {
                    ResourceId = resources[1].Id,
                    StartTime = DateTime.Now.AddHours(2),
                    EndTime = DateTime.Now.AddHours(4),
                    BookedBy = "Bhekani Masinga",
                    Purpose = "Client Meeting"
                }
            };

            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}



/*using InternalResourceBookingSystem.Models;

namespace InternalResourceBookingSystem.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Skip seeding if already exists
            if (context.Resources.Any()) return;

            var resources = new List<Resource>
            {
                new Resource
                {
                    Name = "Meeting Room Alpha",
                    Description = "Large room with projector and whiteboard",
                    Location = "3rd Floor, West Wing",
                    Capacity = 10,
                    IsAvailable = true
                },
                new Resource
                {
                    Name = "Company Car 1",
                    Description = "Compact sedan for business use",
                    Location = "Parking Bay 5",
                    Capacity = 4,
                    IsAvailable = true
                }
            };

            context.Resources.AddRange(resources);
            context.SaveChanges();

            var bookings = new List<Booking>
            {
                new Booking
                {
                    ResourceId = resources[0].Id,
                    StartTime = DateTime.Today.AddHours(9),
                    EndTime = DateTime.Today.AddHours(10),
                    BookedBy = "Bhekani Masinga",
                    Purpose = "Team Stand-up"
                },
                new Booking
                {
                    ResourceId = resources[1].Id,
                    StartTime = DateTime.Now.AddHours(2),
                    EndTime = DateTime.Now.AddHours(4),
                    BookedBy = "Bhekani Masinga",
                    Purpose = "Client Meeting"
                }
            };

            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}
*/