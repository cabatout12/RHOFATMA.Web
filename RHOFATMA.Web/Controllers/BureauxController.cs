using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class BureauxController : Controller
{
    private readonly ApplicationDbContext _context;

    public BureauxController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var bureaux = _context.TblBureaus.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            bureaux = bureaux.Where(b =>
                b.NomBureau.Contains(search) ||
                (b.Adresse != null && b.Adresse.Contains(search)));
        }

        return View(await bureaux.OrderBy(b => b.NomBureau).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var bureau = await _context.TblBureaus.AsNoTracking().FirstOrDefaultAsync(b => b.IdBureau == id);
        return bureau == null ? NotFound() : View(bureau);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomBureau,Adresse")] TblBureau bureau)
    {
        if (!ModelState.IsValid) return View(bureau);

        _context.Add(bureau);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var bureau = await _context.TblBureaus.FindAsync(id);
        return bureau == null ? NotFound() : View(bureau);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdBureau,NomBureau,Adresse")] TblBureau bureau)
    {
        if (id != bureau.IdBureau) return NotFound();
        if (!ModelState.IsValid) return View(bureau);

        _context.Update(bureau);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var bureau = await _context.TblBureaus.AsNoTracking().FirstOrDefaultAsync(b => b.IdBureau == id);
        return bureau == null ? NotFound() : View(bureau);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var bureau = await _context.TblBureaus.FindAsync(id);
        if (bureau != null)
        {
            _context.TblBureaus.Remove(bureau);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
