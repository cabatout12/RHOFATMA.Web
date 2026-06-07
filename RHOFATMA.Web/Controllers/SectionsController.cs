using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class SectionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SectionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var sections = _context.TblSections.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            sections = sections.Where(s => s.NomSection.Contains(search));
        }

        return View(await sections.OrderBy(s => s.NomSection).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var section = await _context.TblSections.AsNoTracking().FirstOrDefaultAsync(s => s.IdSection == id);
        return section == null ? NotFound() : View(section);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomSection")] TblSection section)
    {
        if (!ModelState.IsValid) return View(section);

        _context.Add(section);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var section = await _context.TblSections.FindAsync(id);
        return section == null ? NotFound() : View(section);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdSection,NomSection")] TblSection section)
    {
        if (id != section.IdSection) return NotFound();
        if (!ModelState.IsValid) return View(section);

        _context.Update(section);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var section = await _context.TblSections.AsNoTracking().FirstOrDefaultAsync(s => s.IdSection == id);
        return section == null ? NotFound() : View(section);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var section = await _context.TblSections.FindAsync(id);
        if (section != null)
        {
            _context.TblSections.Remove(section);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
