using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class DirectionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DirectionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var directions = _context.TblDirections.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            directions = directions.Where(d => d.NomDirection.Contains(search));
        }

        return View(await directions.OrderBy(d => d.NomDirection).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var direction = await _context.TblDirections.AsNoTracking().FirstOrDefaultAsync(d => d.IdDirection == id);
        return direction == null ? NotFound() : View(direction);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomDirection")] TblDirection direction)
    {
        if (!ModelState.IsValid) return View(direction);

        _context.Add(direction);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var direction = await _context.TblDirections.FindAsync(id);
        return direction == null ? NotFound() : View(direction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdDirection,NomDirection")] TblDirection direction)
    {
        if (id != direction.IdDirection) return NotFound();
        if (!ModelState.IsValid) return View(direction);

        _context.Update(direction);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var direction = await _context.TblDirections.AsNoTracking().FirstOrDefaultAsync(d => d.IdDirection == id);
        return direction == null ? NotFound() : View(direction);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var direction = await _context.TblDirections.FindAsync(id);
        if (direction != null)
        {
            _context.TblDirections.Remove(direction);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
