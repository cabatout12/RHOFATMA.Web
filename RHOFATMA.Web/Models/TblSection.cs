using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblSection
{
    public int IdSection { get; set; }

    public string NomSection { get; set; } = null!;

    public virtual ICollection<TblEmploye> TblEmployes { get; set; } = new List<TblEmploye>();
}
