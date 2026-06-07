using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class PrimesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PrimesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var primes = _context.TblPrims.Include(p => p.IdEmployeNavigation).Include(p => p.IdUserNavigation).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            primes = primes.Where(p =>
                p.TypePrim.Contains(search) ||
                p.IdEmployeNavigation.NomEmploye.Contains(search) ||
                p.IdEmployeNavigation.PrenomEmploye.Contains(search));
        }

        return View(await primes.OrderByDescending(p => p.DatePrim).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var prime = await _context.TblPrims.Include(p => p.IdEmployeNavigation).Include(p => p.IdUserNavigation).AsNoTracking().FirstOrDefaultAsync(p => p.IdPrim == id);
        return prime == null ? NotFound() : View(prime);
    }

    public IActionResult Create()
    {
        PopulateSelectLists();
        return View(new TblPrim { DatePrim = DateOnly.FromDateTime(DateTime.Today), AjouterLe = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdEmploye,TypePrim,MontantPrime,DatePrim,IdUser")] TblPrim prime)
    {
        if (!ModelState.IsValid)
        {
            PopulateSelectLists(prime);
            return View(prime);
        }

        prime.AjouterLe = DateTime.Now;
        _context.Add(prime);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var prime = await _context.TblPrims.FindAsync(id);
        if (prime == null) return NotFound();
        PopulateSelectLists(prime);
        return View(prime);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdPrim,IdEmploye,TypePrim,MontantPrime,DatePrim,AjouterPar,AjouterLe,IdUser")] TblPrim prime)
    {
        if (id != prime.IdPrim) return NotFound();
        if (!ModelState.IsValid)
        {
            PopulateSelectLists(prime);
            return View(prime);
        }

        prime.ModifierLe = DateTime.Now;
        _context.Update(prime);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var prime = await _context.TblPrims.Include(p => p.IdEmployeNavigation).AsNoTracking().FirstOrDefaultAsync(p => p.IdPrim == id);
        return prime == null ? NotFound() : View(prime);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var prime = await _context.TblPrims.FindAsync(id);
        if (prime != null)
        {
            _context.TblPrims.Remove(prime);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateSelectLists(TblPrim? prime = null)
    {
        ViewData["IdEmploye"] = new SelectList(_context.TblEmployes.AsNoTracking().OrderBy(e => e.NomEmploye), "IdEmploye", "NomEmploye", prime?.IdEmploye);
        ViewData["IdUser"] = new SelectList(_context.TblUsers.AsNoTracking().OrderBy(u => u.UserEmail), "IdUser", "UserEmail", prime?.IdUser);
    }
}
