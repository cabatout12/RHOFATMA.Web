using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class RolesController : Controller
{
    private readonly ApplicationDbContext _context;

    public RolesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var roles = _context.TblRoles.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            roles = roles.Where(r =>
                r.NomRole.Contains(search) ||
                (r.DescriptionRole != null && r.DescriptionRole.Contains(search)));
        }

        return View(await roles.OrderBy(r => r.NomRole).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var role = await _context.TblRoles.AsNoTracking().FirstOrDefaultAsync(r => r.IdRole == id);
        return role == null ? NotFound() : View(role);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomRole,DescriptionRole")] TblRole role)
    {
        if (!ModelState.IsValid) return View(role);
        _context.Add(role);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var role = await _context.TblRoles.FindAsync(id);
        return role == null ? NotFound() : View(role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdRole,NomRole,DescriptionRole")] TblRole role)
    {
        if (id != role.IdRole) return NotFound();
        if (!ModelState.IsValid) return View(role);
        _context.Update(role);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var role = await _context.TblRoles.AsNoTracking().FirstOrDefaultAsync(r => r.IdRole == id);
        return role == null ? NotFound() : View(role);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var role = await _context.TblRoles.FindAsync(id);
        if (role != null)
        {
            _context.TblRoles.Remove(role);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
