using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblConge
{
    public int IdConge { get; set; }

    public int IdEmploye { get; set; }

    public DateOnly DateDebut { get; set; }

    public DateOnly DateFin { get; set; }

    public string TypeConge { get; set; } = null!;

    public string? Motif { get; set; }

    public string Statut { get; set; } = null!;

    public DateOnly DateDemande { get; set; }

    public string? ApprouvePar { get; set; }

    public DateOnly? DateApprouve { get; set; }

    public string? Remarques { get; set; }

    public int? JoursAnnuels { get; set; }

    public int? JoursUtilises { get; set; }

    public int? JoursRestant { get; set; }

    public int NombreJoursDemande { get; set; }

    public int? IdUser { get; set; }

    public string? AjouterPar { get; set; }

    public DateTime AjouterLe { get; set; }

    public string? ModifierPar { get; set; }

    public DateTime? ModifierLe { get; set; }

    public virtual TblEmploye IdEmployeNavigation { get; set; } = null!;

    public virtual TblUser? IdUserNavigation { get; set; }
}
