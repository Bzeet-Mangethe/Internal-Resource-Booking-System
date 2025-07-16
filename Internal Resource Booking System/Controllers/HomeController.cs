using InternalResourceBookingSystem.Data; // Adjust namespace
using InternalResourceBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize] // 👈 Authorising signed in user

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var now = DateTime.Now;

        var todaysBookings = await _context.Bookings
            .Include(b => b.Resource)
            .Where(b => b.StartTime.Date == today)
            .OrderBy(b => b.StartTime)
            .ToListAsync();

       var upcomingBookings = await _context.Bookings
            .Include(b => b.Resource)
            .Where(b => b.StartTime > now)
            .OrderBy(b => b.StartTime)
            .Take(10)
            .ToListAsync();

        ViewBag.TodaysBookings = todaysBookings;
        ViewBag.UpcomingBookings = upcomingBookings;

        return View();
    }
}
