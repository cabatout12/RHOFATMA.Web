using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class PostesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var postes = _context.TblPostes.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            postes = postes.Where(p =>
                p.NomPoste.Contains(search) ||
                (p.DescPoste != null && p.DescPoste.Contains(search)));
        }

        return View(await postes.OrderBy(p => p.NomPoste).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var poste = await _context.TblPostes.AsNoTracking().FirstOrDefaultAsync(p => p.IdPoste == id);
        return poste == null ? NotFound() : View(poste);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomPoste,DescPoste")] TblPoste poste)
    {
        if (!ModelState.IsValid) return View(poste);
        _context.Add(poste);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var poste = await _context.TblPostes.FindAsync(id);
        return poste == null ? NotFound() : View(poste);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdPoste,NomPoste,DescPoste")] TblPoste poste)
    {
        if (id != poste.IdPoste) return NotFound();
        if (!ModelState.IsValid) return View(poste);
        _context.Update(poste);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var poste = await _context.TblPostes.AsNoTracking().FirstOrDefaultAsync(p => p.IdPoste == id);
        return poste == null ? NotFound() : View(poste);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var poste = await _context.TblPostes.FindAsync(id);
        if (poste != null)
        {
            _context.TblPostes.Remove(poste);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
