using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblPaie
{
    public int IdPaie { get; set; }

    public int IdEmploye { get; set; }

    public int Mois { get; set; }

    public int Annee { get; set; }

    public decimal SalaireBase { get; set; }

    public decimal TotalPrimes { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal? SalaireNet { get; set; }

    public DateOnly? DatePaiement { get; set; }

    public string Statut { get; set; } = null!;

    public DateTime AjouterLe { get; set; }

    public virtual TblEmploye IdEmployeNavigation { get; set; } = null!;
}
