using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblAuditGenerale
{
    public int IdAuditGenerale { get; set; }

    public string? NomTable { get; set; }

    public int? IdEnregistrement { get; set; }

    public string? ChampModifier { get; set; }

    public string? ValeurAvant { get; set; }

    public string? NouveauValeur { get; set; }

    public string? ActionType { get; set; }

    public DateTime DateAction { get; set; }

    public int? UserId { get; set; }

    public virtual TblUser? User { get; set; }
}
