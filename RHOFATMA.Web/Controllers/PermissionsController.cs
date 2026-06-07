using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class PermissionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PermissionsController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var permissions = _context.TblPermissions.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            permissions = permissions.Where(p =>
                p.CodePermission.Contains(search) ||
                (p.DescriptionPermission != null && p.DescriptionPermission.Contains(search)));
        }

        return View(await permissions.OrderBy(p => p.CodePermission).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var permission = await _context.TblPermissions.AsNoTracking().FirstOrDefaultAsync(p => p.IdPermission == id);
        return permission == null ? NotFound() : View(permission);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CodePermission,DescriptionPermission")] TblPermission permission)
    {
        if (!ModelState.IsValid) return View(permission);
        _context.Add(permission);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var permission = await _context.TblPermissions.FindAsync(id);
        return permission == null ? NotFound() : View(permission);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdPermission,CodePermission,DescriptionPermission")] TblPermission permission)
    {
        if (id != permission.IdPermission) return NotFound();
        if (!ModelState.IsValid) return View(permission);
        _context.Update(permission);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var permission = await _context.TblPermissions.AsNoTracking().FirstOrDefaultAsync(p => p.IdPermission == id);
        return permission == null ? NotFound() : View(permission);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var permission = await _context.TblPermissions.FindAsync(id);
        if (permission != null)
        {
            _context.TblPermissions.Remove(permission);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
