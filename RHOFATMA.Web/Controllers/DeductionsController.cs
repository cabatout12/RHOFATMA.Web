using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class DeductionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DeductionsController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var deductions = _context.TblDeductions.Include(d => d.IdEmployeNavigation).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            deductions = deductions.Where(d =>
                d.TypeDeduction.Contains(search) ||
                (d.Motif != null && d.Motif.Contains(search)) ||
                d.IdEmployeNavigation.NomEmploye.Contains(search) ||
                d.IdEmployeNavigation.PrenomEmploye.Contains(search));
        }

        return View(await deductions.OrderByDescending(d => d.DateDeduction).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var deduction = await _context.TblDeductions.Include(d => d.IdEmployeNavigation).AsNoTracking().FirstOrDefaultAsync(d => d.IdDeduction == id);
        return deduction == null ? NotFound() : View(deduction);
    }

    public IActionResult Create()
    {
        PopulateEmployes();
        return View(new TblDeduction { DateDeduction = DateOnly.FromDateTime(DateTime.Today), AjouterLe = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdEmploye,TypeDeduction,MontantDeduction,DateDeduction,Motif")] TblDeduction deduction)
    {
        if (!ModelState.IsValid)
        {
            PopulateEmployes(deduction);
            return View(deduction);
        }

        deduction.AjouterLe = DateTime.Now;
        _context.Add(deduction);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var deduction = await _context.TblDeductions.FindAsync(id);
        if (deduction == null) return NotFound();
        PopulateEmployes(deduction);
        return View(deduction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdDeduction,IdEmploye,TypeDeduction,MontantDeduction,DateDeduction,Motif,AjouterLe")] TblDeduction deduction)
    {
        if (id != deduction.IdDeduction) return NotFound();
        if (!ModelState.IsValid)
        {
            PopulateEmployes(deduction);
            return View(deduction);
        }

        _context.Update(deduction);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var deduction = await _context.TblDeductions.Include(d => d.IdEmployeNavigation).AsNoTracking().FirstOrDefaultAsync(d => d.IdDeduction == id);
        return deduction == null ? NotFound() : View(deduction);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deduction = await _context.TblDeductions.FindAsync(id);
        if (deduction != null)
        {
            _context.TblDeductions.Remove(deduction);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateEmployes(TblDeduction? deduction = null)
    {
        ViewData["IdEmploye"] = new SelectList(_context.TblEmployes.AsNoTracking().OrderBy(e => e.NomEmploye), "IdEmploye", "NomEmploye", deduction?.IdEmploye);
    }
}
