using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalResourceBookingSystem.Data;
using InternalResourceBookingSystem.Models;

public class ResourceController : Controller
{
    private readonly ApplicationDbContext _context;

    public ResourceController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchName)
    {
        var resources = from r in _context.Resources
                        select r;

        if (!string.IsNullOrEmpty(searchName))
        {
            resources = resources.Where(r => r.Name.Contains(searchName));
        }

        return View(await resources.ToListAsync());
    }


    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var resource = await _context.Resources.FirstOrDefaultAsync(r => r.Id == id);
        if (resource == null) return NotFound();

        return View(resource);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Resource resource)
    {
        if (ModelState.IsValid)
        {
            _context.Add(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(resource);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var resource = await _context.Resources.FindAsync(id);
        if (resource == null) return NotFound();

        return View(resource);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Resource resource)
    {
        if (id != resource.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(resource);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Resources.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(resource);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var resource = await _context.Resources.FirstOrDefaultAsync(r => r.Id == id);
        if (resource == null) return NotFound();

        return View(resource);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var resource = await _context.Resources.FindAsync(id);
        _context.Resources.Remove(resource);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
