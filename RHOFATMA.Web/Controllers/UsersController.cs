using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var users = _context.TblUsers.Include(u => u.IdRoleNavigation).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            users = users.Where(u =>
                u.UserEmail.Contains(search) ||
                u.Statut.Contains(search) ||
                (u.IdRoleNavigation != null && u.IdRoleNavigation.NomRole.Contains(search)));
        }

        return View(await users.OrderBy(u => u.UserEmail).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var user = await _context.TblUsers.Include(u => u.IdRoleNavigation).AsNoTracking().FirstOrDefaultAsync(u => u.IdUser == id);
        return user == null ? NotFound() : View(user);
    }

    public IActionResult Create()
    {
        PopulateRoles();
        return View(new TblUser { Statut = "Actif", DateCreation = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UserEmail,PasswordHash,IdRole,Statut")] TblUser user)
    {
        if (!ModelState.IsValid)
        {
            PopulateRoles(user);
            return View(user);
        }

        user.DateCreation = DateTime.Now;
        _context.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var user = await _context.TblUsers.FindAsync(id);
        if (user == null) return NotFound();
        PopulateRoles(user);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdUser,UserEmail,PasswordHash,IdRole,AjoutePar,DateCreation,Statut")] TblUser user)
    {
        if (id != user.IdUser) return NotFound();
        if (!ModelState.IsValid)
        {
            PopulateRoles(user);
            return View(user);
        }

        user.DateModification = DateTime.Now;
        _context.Update(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var user = await _context.TblUsers.Include(u => u.IdRoleNavigation).AsNoTracking().FirstOrDefaultAsync(u => u.IdUser == id);
        return user == null ? NotFound() : View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.TblUsers.FindAsync(id);
        if (user != null)
        {
            _context.TblUsers.Remove(user);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateRoles(TblUser? user = null)
    {
        ViewData["IdRole"] = new SelectList(_context.TblRoles.AsNoTracking(), "IdRole", "NomRole", user?.IdRole);
    }
}
