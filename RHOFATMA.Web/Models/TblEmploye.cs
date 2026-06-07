using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblEmploye
{
    public int IdEmploye { get; set; }

    public string CodeEmploye { get; set; } = null!;

    public string? CinNif { get; set; }

    public string NomEmploye { get; set; } = null!;

    public string PrenomEmploye { get; set; } = null!;

    public DateOnly? DateNaissance { get; set; }

    public string? LieuNaissance { get; set; }

    public string? Sexe { get; set; }

    public string? Adresse { get; set; }

    public string? Telephone { get; set; }

    public string? Email { get; set; }

    public DateOnly? DateEmbaucheEmploye { get; set; }

    public decimal? Salaire { get; set; }

    public string? Diplome { get; set; }

    public int? IdPoste { get; set; }

    public int? IdBureau { get; set; }

    public int? IdSection { get; set; }

    public int? IdDirection { get; set; }

    public int? IdService { get; set; }

    public int? IdFormation { get; set; }

    public int? IdUser { get; set; }

    public string Statut { get; set; } = null!;

    public bool IsApprouve { get; set; }

    public string? AjouterPar { get; set; }

    public DateTime AjouterLe { get; set; }

    public string? ModifierPar { get; set; }

    public DateTime? ModifierLe { get; set; }

    public virtual TblBureau? IdBureauNavigation { get; set; }

    public virtual TblDirection? IdDirectionNavigation { get; set; }

    public virtual TblFormation? IdFormationNavigation { get; set; }

    public virtual TblPoste? IdPosteNavigation { get; set; }

    public virtual TblSection? IdSectionNavigation { get; set; }

    public virtual TblService? IdServiceNavigation { get; set; }

    public virtual TblUser? IdUserNavigation { get; set; }

    public virtual ICollection<TblAbsentRetard> TblAbsentRetards { get; set; } = new List<TblAbsentRetard>();

    public virtual ICollection<TblConge> TblConges { get; set; } = new List<TblConge>();

    public virtual ICollection<TblContrat> TblContrats { get; set; } = new List<TblContrat>();

    public virtual ICollection<TblDeduction> TblDeductions { get; set; } = new List<TblDeduction>();

    public virtual ICollection<TblDocumentEmploye> TblDocumentEmployes { get; set; } = new List<TblDocumentEmploye>();

    public virtual ICollection<TblPaie> TblPaies { get; set; } = new List<TblPaie>();

    public virtual ICollection<TblPresence> TblPresences { get; set; } = new List<TblPresence>();

    public virtual ICollection<TblPrim> TblPrims { get; set; } = new List<TblPrim>();
}
