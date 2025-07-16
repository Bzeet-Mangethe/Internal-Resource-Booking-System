using InternalResourceBookingSystem.Data;
using InternalResourceBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Show all bookings
    public async Task<IActionResult> Index(string searchResource, DateTime? searchDate)
    {
        var query = _context.Bookings.Include(b => b.Resource).AsQueryable();

        if (!string.IsNullOrEmpty(searchResource))
        {
            query = query.Where(b => b.Resource.Name.Contains(searchResource));
        }

        if (searchDate.HasValue)
        {
            query = query.Where(b => b.StartTime.Date == searchDate.Value.Date);
        }

        var filteredBookings = await query.OrderByDescending(b => b.StartTime).ToListAsync();

        return View(filteredBookings);
    }


    // Show booking form
    public IActionResult Create()
    {
        ViewBag.Resources = _context.Resources
            .Where(r => r.IsAvailable)
            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            })
            .ToList();

        return View();
    }

    // Handle form submission
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking booking)
    {
        if (booking.StartTime >= booking.EndTime)
        {
            ModelState.AddModelError("", "End time must be after start time.");
        }

        // Conflict check
        bool conflict = await _context.Bookings.AnyAsync(b =>
            b.ResourceId == booking.ResourceId &&
            ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
             (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
             (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime))
        );

        if (conflict)
        {
            ModelState.AddModelError("", "⚠️ This resource is already booked during the selected time.");
        }

        if (ModelState.IsValid)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Resources = _context.Resources
            .Where(r => r.IsAvailable)
            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            })
            .ToList();

        return View(booking);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings
            .Include(b => b.Resource)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null) return NotFound();

        return View(booking);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        ViewBag.Resources = _context.Resources
            .Where(r => r.IsAvailable)
            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            })
            .ToList();

        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Booking booking)
    {
        if (id != booking.Id) return NotFound();

        if (booking.StartTime >= booking.EndTime)
        {
            ModelState.AddModelError("", "End time must be after start time.");
        }

        bool conflict = await _context.Bookings.AnyAsync(b =>
            b.ResourceId == booking.ResourceId &&
            b.Id != booking.Id &&
            ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
             (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
             (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)));

        if (conflict)
        {
            ModelState.AddModelError("", "⚠️ Conflict: Resource already booked during selected time.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        ViewBag.Resources = _context.Resources
            .Where(r => r.IsAvailable)
            .Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            })
            .ToList();

        return View(booking);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings
            .Include(b => b.Resource)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null) return NotFound();

        return View(booking);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

}
