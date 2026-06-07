using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblDirection
{
    public int IdDirection { get; set; }

    public string NomDirection { get; set; } = null!;

    public virtual ICollection<TblEmploye> TblEmployes { get; set; } = new List<TblEmploye>();
}
