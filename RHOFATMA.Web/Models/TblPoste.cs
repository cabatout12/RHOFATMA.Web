using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblPoste
{
    public int IdPoste { get; set; }

    public string NomPoste { get; set; } = null!;

    public string? DescPoste { get; set; }

    public virtual ICollection<TblEmploye> TblEmployes { get; set; } = new List<TblEmploye>();
}
