using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InternalResourceBookingSystem.Data;
using InternalResourceBookingSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity + Roles
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Needed for Identity UI pages
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware setup
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed roles and admin user
        await SeedIdentityAsync(userManager, roleManager);

        // Seed resources
        SeedResources(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

async Task SeedIdentityAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
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

void SeedResources(ApplicationDbContext context)
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

app.Run();




/*using Microsoft.EntityFrameworkCore;
using InternalResourceBookingSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ STEP 4: Add ApplicationDbContext for EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Seed(context);
}


app.Run();
*/