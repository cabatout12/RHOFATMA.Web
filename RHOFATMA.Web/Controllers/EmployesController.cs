using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHOFATMA.Web.Data;
using RHOFATMA.Web.Models;

namespace RHOFATMA.Web.Controllers;

public class EmployesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search)
    {
        ViewData["CurrentSearch"] = search;

        var employes = _context.TblEmployes
            .Include(e => e.IdBureauNavigation)
            .Include(e => e.IdDirectionNavigation)
            .Include(e => e.IdPosteNavigation)
            .Include(e => e.IdServiceNavigation)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            employes = employes.Where(e =>
                e.CodeEmploye.Contains(search) ||
                e.NomEmploye.Contains(search) ||
                e.PrenomEmploye.Contains(search) ||
                (e.Telephone != null && e.Telephone.Contains(search)) ||
                (e.Email != null && e.Email.Contains(search)));
        }

        return View(await employes.OrderBy(e => e.NomEmploye).ThenBy(e => e.PrenomEmploye).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employe = await _context.TblEmployes
            .Include(e => e.IdBureauNavigation)
            .Include(e => e.IdDirectionNavigation)
            .Include(e => e.IdFormationNavigation)
            .Include(e => e.IdPosteNavigation)
            .Include(e => e.IdSectionNavigation)
            .Include(e => e.IdServiceNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEmploye == id);

        if (employe == null)
        {
            return NotFound();
        }

        return View(employe);
    }

    public IActionResult Create()
    {
        PopulateSelectLists();
        return View(new TblEmploye
        {
            Statut = "Actif",
            AjouterLe = DateTime.Now
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CodeEmploye,CinNif,NomEmploye,PrenomEmploye,DateNaissance,LieuNaissance,Sexe,Adresse,Telephone,Email,DateEmbaucheEmploye,Salaire,Diplome,IdPoste,IdBureau,IdSection,IdDirection,IdService,IdFormation,Statut,IsApprouve")] TblEmploye employe)
    {
        if (ModelState.IsValid)
        {
            employe.AjouterLe = DateTime.Now;
            _context.Add(employe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PopulateSelectLists(employe);
        return View(employe);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employe = await _context.TblEmployes.FindAsync(id);
        if (employe == null)
        {
            return NotFound();
        }

        PopulateSelectLists(employe);
        return View(employe);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdEmploye,CodeEmploye,CinNif,NomEmploye,PrenomEmploye,DateNaissance,LieuNaissance,Sexe,Adresse,Telephone,Email,DateEmbaucheEmploye,Salaire,Diplome,IdPoste,IdBureau,IdSection,IdDirection,IdService,IdFormation,Statut,IsApprouve,AjouterPar,AjouterLe")] TblEmploye employe)
    {
        if (id != employe.IdEmploye)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                employe.ModifierLe = DateTime.Now;
                _context.Update(employe);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeExists(employe.IdEmploye))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        PopulateSelectLists(employe);
        return View(employe);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employe = await _context.TblEmployes
            .Include(e => e.IdPosteNavigation)
            .Include(e => e.IdServiceNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEmploye == id);

        if (employe == null)
        {
            return NotFound();
        }

        return View(employe);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employe = await _context.TblEmployes.FindAsync(id);
        if (employe != null)
        {
            _context.TblEmployes.Remove(employe);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool EmployeExists(int id)
    {
        return _context.TblEmployes.Any(e => e.IdEmploye == id);
    }

    private void PopulateSelectLists(TblEmploye? employe = null)
    {
        ViewData["IdBureau"] = new SelectList(_context.TblBureaus.AsNoTracking(), "IdBureau", "NomBureau", employe?.IdBureau);
        ViewData["IdDirection"] = new SelectList(_context.TblDirections.AsNoTracking(), "IdDirection", "NomDirection", employe?.IdDirection);
        ViewData["IdFormation"] = new SelectList(_context.TblFormations.AsNoTracking(), "IdFormation", "NomFormation", employe?.IdFormation);
        ViewData["IdPoste"] = new SelectList(_context.TblPostes.AsNoTracking(), "IdPoste", "NomPoste", employe?.IdPoste);
        ViewData["IdSection"] = new SelectList(_context.TblSections.AsNoTracking(), "IdSection", "NomSection", employe?.IdSection);
        ViewData["IdService"] = new SelectList(_context.TblServices.AsNoTracking(), "IdService", "NomService", employe?.IdService);
    }
}
