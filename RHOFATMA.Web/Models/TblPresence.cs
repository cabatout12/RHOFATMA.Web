using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblPresence
{
    public int IdPresence { get; set; }

    public int IdEmploye { get; set; }

    public DateOnly DatePresence { get; set; }

    public TimeOnly? HeureArrivee { get; set; }

    public TimeOnly? HeureDepart { get; set; }

    public string Statut { get; set; } = null!;

    public string? Remarque { get; set; }

    public string? AjouterPar { get; set; }

    public DateTime AjouterLe { get; set; }

    public virtual TblEmploye IdEmployeNavigation { get; set; } = null!;
}
