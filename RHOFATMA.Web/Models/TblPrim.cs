using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblPrim
{
    public int IdPrim { get; set; }

    public int IdEmploye { get; set; }

    public string TypePrim { get; set; } = null!;

    public decimal MontantPrime { get; set; }

    public DateOnly DatePrim { get; set; }

    public string? AjouterPar { get; set; }

    public DateTime AjouterLe { get; set; }

    public string? ModifierPar { get; set; }

    public DateTime? ModifierLe { get; set; }

    public int? IdUser { get; set; }

    public virtual TblEmploye IdEmployeNavigation { get; set; } = null!;

    public virtual TblUser? IdUserNavigation { get; set; }
}
