using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblDocumentEmploye
{
    public int IdDocumentEmploye { get; set; }

    public int IdEmploye { get; set; }

    public string TypeDocument { get; set; } = null!;

    public string NomFichier { get; set; } = null!;

    public string? CheminFichier { get; set; }

    public DateTime DateAjout { get; set; }

    public string? AjouterPar { get; set; }

    public virtual TblEmploye IdEmployeNavigation { get; set; } = null!;
}
