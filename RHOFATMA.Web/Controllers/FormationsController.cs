using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class FormationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public FormationsController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var formations = _context.TblFormations.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            formations = formations.Where(f => f.NomFormation.Contains(search));
        }

        return View(await formations.OrderBy(f => f.NomFormation).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var formation = await _context.TblFormations.AsNoTracking().FirstOrDefaultAsync(f => f.IdFormation == id);
        return formation == null ? NotFound() : View(formation);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomFormation")] TblFormation formation)
    {
        if (!ModelState.IsValid) return View(formation);
        _context.Add(formation);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var formation = await _context.TblFormations.FindAsync(id);
        return formation == null ? NotFound() : View(formation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdFormation,NomFormation")] TblFormation formation)
    {
        if (id != formation.IdFormation) return NotFound();
        if (!ModelState.IsValid) return View(formation);
        _context.Update(formation);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var formation = await _context.TblFormations.AsNoTracking().FirstOrDefaultAsync(f => f.IdFormation == id);
        return formation == null ? NotFound() : View(formation);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var formation = await _context.TblFormations.FindAsync(id);
        if (formation != null)
        {
            _context.TblFormations.Remove(formation);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
