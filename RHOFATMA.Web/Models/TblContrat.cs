using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblContrat
{
    public int IdContrat { get; set; }

    public int IdEmploye { get; set; }

    public string TypeContrat { get; set; } = null!;

    public DateOnly DateDebut { get; set; }

    public DateOnly? DateFin { get; set; }

    public decimal SalaireBase { get; set; }

    public string Statut { get; set; } = null!;

    public string? AjouterPar { get; set; }

    public DateTime AjouterLe { get; set; }

    public string? ModifierPar { get; set; }

    public DateTime? ModifierLe { get; set; }

    public virtual TblEmploye IdEmployeNavigation { get; set; } = null!;
}
