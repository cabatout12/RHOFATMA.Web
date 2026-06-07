using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblBureau
{
    public int IdBureau { get; set; }

    public string NomBureau { get; set; } = null!;

    public string? Adresse { get; set; }

    public virtual ICollection<TblEmploye> TblEmployes { get; set; } = new List<TblEmploye>();
}
