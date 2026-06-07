using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class AbsentRetardsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AbsentRetardsController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var absentRetards = _context.TblAbsentRetards.Include(a => a.IdEmployeNavigation).Include(a => a.IdUserNavigation).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            absentRetards = absentRetards.Where(a =>
                a.Statut.Contains(search) ||
                (a.Motif != null && a.Motif.Contains(search)) ||
                a.IdEmployeNavigation.NomEmploye.Contains(search) ||
                a.IdEmployeNavigation.PrenomEmploye.Contains(search));
        }

        return View(await absentRetards.OrderByDescending(a => a.DateRetard).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var absentRetard = await _context.TblAbsentRetards.Include(a => a.IdEmployeNavigation).Include(a => a.IdUserNavigation).AsNoTracking().FirstOrDefaultAsync(a => a.IdAbsentRetard == id);
        return absentRetard == null ? NotFound() : View(absentRetard);
    }

    public IActionResult Create()
    {
        PopulateSelectLists();
        return View(new TblAbsentRetard { DateRetard = DateOnly.FromDateTime(DateTime.Today), Statut = "Absent", AjouterLe = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdEmploye,DateRetard,Statut,Motif,IdUser")] TblAbsentRetard absentRetard)
    {
        if (!ModelState.IsValid)
        {
            PopulateSelectLists(absentRetard);
            return View(absentRetard);
        }

        absentRetard.AjouterLe = DateTime.Now;
        _context.Add(absentRetard);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var absentRetard = await _context.TblAbsentRetards.FindAsync(id);
        if (absentRetard == null) return NotFound();
        PopulateSelectLists(absentRetard);
        return View(absentRetard);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdAbsentRetard,IdEmploye,DateRetard,Statut,Motif,AjouterPar,AjouterLe,IdUser")] TblAbsentRetard absentRetard)
    {
        if (id != absentRetard.IdAbsentRetard) return NotFound();
        if (!ModelState.IsValid)
        {
            PopulateSelectLists(absentRetard);
            return View(absentRetard);
        }

        absentRetard.ModifierLe = DateTime.Now;
        _context.Update(absentRetard);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var absentRetard = await _context.TblAbsentRetards.Include(a => a.IdEmployeNavigation).AsNoTracking().FirstOrDefaultAsync(a => a.IdAbsentRetard == id);
        return absentRetard == null ? NotFound() : View(absentRetard);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var absentRetard = await _context.TblAbsentRetards.FindAsync(id);
        if (absentRetard != null)
        {
            _context.TblAbsentRetards.Remove(absentRetard);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateSelectLists(TblAbsentRetard? absentRetard = null)
    {
        ViewData["IdEmploye"] = new SelectList(_context.TblEmployes.AsNoTracking().OrderBy(e => e.NomEmploye), "IdEmploye", "NomEmploye", absentRetard?.IdEmploye);
        ViewData["IdUser"] = new SelectList(_context.TblUsers.AsNoTracking().OrderBy(u => u.UserEmail), "IdUser", "UserEmail", absentRetard?.IdUser);
    }
}
