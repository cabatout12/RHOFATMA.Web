using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblPermission
{
    public int IdPermission { get; set; }

    public string CodePermission { get; set; } = null!;

    public string? DescriptionPermission { get; set; }

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();
}
