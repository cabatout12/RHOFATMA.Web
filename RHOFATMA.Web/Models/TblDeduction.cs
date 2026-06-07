using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblDeduction
{
    public int IdDeduction { get; set; }

    public int IdEmploye { get; set; }

    public string TypeDeduction { get; set; } = null!;

    public decimal MontantDeduction { get; set; }

    public DateOnly DateDeduction { get; set; }

    public string? Motif { get; set; }

    public DateTime AjouterLe { get; set; }

    public virtual TblEmploye IdEmployeNavigation { get; set; } = null!;
}
