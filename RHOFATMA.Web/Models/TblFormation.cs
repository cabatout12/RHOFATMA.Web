using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblFormation
{
    public int IdFormation { get; set; }

    public string NomFormation { get; set; } = null!;

    public virtual ICollection<TblEmploye> TblEmployes { get; set; } = new List<TblEmploye>();
}
