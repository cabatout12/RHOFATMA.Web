using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class ServicesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ServicesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;
        var services = _context.TblServices.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            services = services.Where(s => s.NomService.Contains(search));
        }

        return View(await services.OrderBy(s => s.NomService).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var service = await _context.TblServices.AsNoTracking().FirstOrDefaultAsync(s => s.IdService == id);
        return service == null ? NotFound() : View(service);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NomService")] TblService service)
    {
        if (!ModelState.IsValid) return View(service);

        _context.Add(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var service = await _context.TblServices.FindAsync(id);
        return service == null ? NotFound() : View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdService,NomService")] TblService service)
    {
        if (id != service.IdService) return NotFound();
        if (!ModelState.IsValid) return View(service);

        _context.Update(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var service = await _context.TblServices.AsNoTracking().FirstOrDefaultAsync(s => s.IdService == id);
        return service == null ? NotFound() : View(service);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var service = await _context.TblServices.FindAsync(id);
        if (service != null)
        {
            _context.TblServices.Remove(service);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
