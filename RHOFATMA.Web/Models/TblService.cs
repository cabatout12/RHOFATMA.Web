using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblService
{
    public int IdService { get; set; }

    public string NomService { get; set; } = null!;

    public virtual ICollection<TblEmploye> TblEmployes { get; set; } = new List<TblEmploye>();
}
