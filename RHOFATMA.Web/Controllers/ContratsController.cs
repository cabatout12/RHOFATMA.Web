using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class ContratsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContratsController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var contrats = _context.TblContrats.Include(c => c.IdEmployeNavigation).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            contrats = contrats.Where(c =>
                c.TypeContrat.Contains(search) ||
                c.Statut.Contains(search) ||
                c.IdEmployeNavigation.NomEmploye.Contains(search) ||
                c.IdEmployeNavigation.PrenomEmploye.Contains(search));
        }

        return View(await contrats.OrderByDescending(c => c.DateDebut).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var contrat = await _context.TblContrats.Include(c => c.IdEmployeNavigation).AsNoTracking().FirstOrDefaultAsync(c => c.IdContrat == id);
        return contrat == null ? NotFound() : View(contrat);
    }

    public IActionResult Create()
    {
        PopulateEmployes();
        return View(new TblContrat { DateDebut = DateOnly.FromDateTime(DateTime.Today), Statut = "Actif", AjouterLe = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdEmploye,TypeContrat,DateDebut,DateFin,SalaireBase,Statut")] TblContrat contrat)
    {
        if (!ModelState.IsValid)
        {
            PopulateEmployes(contrat);
            return View(contrat);
        }

        contrat.AjouterLe = DateTime.Now;
        _context.Add(contrat);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var contrat = await _context.TblContrats.FindAsync(id);
        if (contrat == null) return NotFound();
        PopulateEmployes(contrat);
        return View(contrat);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdContrat,IdEmploye,TypeContrat,DateDebut,DateFin,SalaireBase,Statut,AjouterPar,AjouterLe")] TblContrat contrat)
    {
        if (id != contrat.IdContrat) return NotFound();
        if (!ModelState.IsValid)
        {
            PopulateEmployes(contrat);
            return View(contrat);
        }

        contrat.ModifierLe = DateTime.Now;
        _context.Update(contrat);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var contrat = await _context.TblContrats.Include(c => c.IdEmployeNavigation).AsNoTracking().FirstOrDefaultAsync(c => c.IdContrat == id);
        return contrat == null ? NotFound() : View(contrat);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var contrat = await _context.TblContrats.FindAsync(id);
        if (contrat != null)
        {
            _context.TblContrats.Remove(contrat);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateEmployes(TblContrat? contrat = null)
    {
        ViewData["IdEmploye"] = new SelectList(_context.TblEmployes.AsNoTracking().OrderBy(e => e.NomEmploye), "IdEmploye", "NomEmploye", contrat?.IdEmploye);
    }
}
