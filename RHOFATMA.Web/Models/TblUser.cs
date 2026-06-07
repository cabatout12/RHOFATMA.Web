using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblUser
{
    public int IdUser { get; set; }

    public string UserEmail { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? IdRole { get; set; }

    public string? AjoutePar { get; set; }

    public DateTime DateCreation { get; set; }

    public string? ModifierPar { get; set; }

    public DateTime? DateModification { get; set; }

    public string Statut { get; set; } = null!;

    public virtual TblRole? IdRoleNavigation { get; set; }

    public virtual ICollection<TblAbsentRetard> TblAbsentRetards { get; set; } = new List<TblAbsentRetard>();

    public virtual ICollection<TblAuditGenerale> TblAuditGenerales { get; set; } = new List<TblAuditGenerale>();

    public virtual ICollection<TblConge> TblConges { get; set; } = new List<TblConge>();

    public virtual ICollection<TblEmploye> TblEmployes { get; set; } = new List<TblEmploye>();

    public virtual ICollection<TblPrim> TblPrims { get; set; } = new List<TblPrim>();
}
