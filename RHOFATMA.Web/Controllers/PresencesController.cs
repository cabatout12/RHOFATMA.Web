using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class PresencesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PresencesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var presences = _context.TblPresences.Include(p => p.IdEmployeNavigation).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            presences = presences.Where(p =>
                p.Statut.Contains(search) ||
                p.IdEmployeNavigation.NomEmploye.Contains(search) ||
                p.IdEmployeNavigation.PrenomEmploye.Contains(search));
        }

        return View(await presences.OrderByDescending(p => p.DatePresence).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var presence = await _context.TblPresences.Include(p => p.IdEmployeNavigation).AsNoTracking().FirstOrDefaultAsync(p => p.IdPresence == id);
        return presence == null ? NotFound() : View(presence);
    }

    public IActionResult Create()
    {
        PopulateEmployes();
        return View(new TblPresence { DatePresence = DateOnly.FromDateTime(DateTime.Today), Statut = "Present", AjouterLe = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdEmploye,DatePresence,HeureArrivee,HeureDepart,Statut,Remarque")] TblPresence presence)
    {
        if (!ModelState.IsValid)
        {
            PopulateEmployes(presence);
            return View(presence);
        }

        presence.AjouterLe = DateTime.Now;
        _context.Add(presence);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var presence = await _context.TblPresences.FindAsync(id);
        if (presence == null) return NotFound();
        PopulateEmployes(presence);
        return View(presence);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdPresence,IdEmploye,DatePresence,HeureArrivee,HeureDepart,Statut,Remarque,AjouterPar,AjouterLe")] TblPresence presence)
    {
        if (id != presence.IdPresence) return NotFound();
        if (!ModelState.IsValid)
        {
            PopulateEmployes(presence);
            return View(presence);
        }

        _context.Update(presence);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var presence = await _context.TblPresences.Include(p => p.IdEmployeNavigation).AsNoTracking().FirstOrDefaultAsync(p => p.IdPresence == id);
        return presence == null ? NotFound() : View(presence);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var presence = await _context.TblPresences.FindAsync(id);
        if (presence != null)
        {
            _context.TblPresences.Remove(presence);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateEmployes(TblPresence? presence = null)
    {
        ViewData["IdEmploye"] = new SelectList(_context.TblEmployes.AsNoTracking().OrderBy(e => e.NomEmploye), "IdEmploye", "NomEmploye", presence?.IdEmploye);
    }
}
