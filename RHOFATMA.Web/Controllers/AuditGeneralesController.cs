using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class AuditGeneralesController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuditGeneralesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var audits = _context.TblAuditGenerales.Include(a => a.User).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            audits = audits.Where(a =>
                (a.NomTable != null && a.NomTable.Contains(search)) ||
                (a.ChampModifier != null && a.ChampModifier.Contains(search)) ||
                (a.ActionType != null && a.ActionType.Contains(search)) ||
                (a.User != null && a.User.UserEmail.Contains(search)));
        }

        return View(await audits.OrderByDescending(a => a.DateAction).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var audit = await _context.TblAuditGenerales.Include(a => a.User).AsNoTracking().FirstOrDefaultAsync(a => a.IdAuditGenerale == id);
        return audit == null ? NotFound() : View(audit);
    }

    public IActionResult Create()
    {
        PopulateUsers();
        return View(new TblAuditGenerale { DateAction = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomTable,IdEnregistrement,ChampModifier,ValeurAvant,NouveauValeur,ActionType,DateAction,UserId")] TblAuditGenerale audit)
    {
        if (!ModelState.IsValid)
        {
            PopulateUsers(audit);
            return View(audit);
        }

        if (audit.DateAction == default)
        {
            audit.DateAction = DateTime.Now;
        }

        _context.Add(audit);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var audit = await _context.TblAuditGenerales.FindAsync(id);
        if (audit == null) return NotFound();
        PopulateUsers(audit);
        return View(audit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdAuditGenerale,NomTable,IdEnregistrement,ChampModifier,ValeurAvant,NouveauValeur,ActionType,DateAction,UserId")] TblAuditGenerale audit)
    {
        if (id != audit.IdAuditGenerale) return NotFound();
        if (!ModelState.IsValid)
        {
            PopulateUsers(audit);
            return View(audit);
        }

        _context.Update(audit);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var audit = await _context.TblAuditGenerales.Include(a => a.User).AsNoTracking().FirstOrDefaultAsync(a => a.IdAuditGenerale == id);
        return audit == null ? NotFound() : View(audit);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var audit = await _context.TblAuditGenerales.FindAsync(id);
        if (audit != null)
        {
            _context.TblAuditGenerales.Remove(audit);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateUsers(TblAuditGenerale? audit = null)
    {
        ViewData["UserId"] = new SelectList(_context.TblUsers.AsNoTracking().OrderBy(u => u.UserEmail), "IdUser", "UserEmail", audit?.UserId);
    }
}
