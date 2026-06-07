using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblAbsentRetard
{
    public int IdAbsentRetard { get; set; }

    public int IdEmploye { get; set; }

    public DateOnly DateRetard { get; set; }

    public string Statut { get; set; } = null!;

    public string? Motif { get; set; }

    public string? AjouterPar { get; set; }

    public DateTime AjouterLe { get; set; }

    public string? ModifierPar { get; set; }

    public DateTime? ModifierLe { get; set; }

    public int? IdUser { get; set; }

    public virtual TblEmploye IdEmployeNavigation { get; set; } = null!;

    public virtual TblUser? IdUserNavigation { get; set; }
}
