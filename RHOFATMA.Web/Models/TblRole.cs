using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblRole
{
    public int IdRole { get; set; }

    public string NomRole { get; set; } = null!;

    public string? DescriptionRole { get; set; }

    public virtual ICollection<TblRolePermission> TblRolePermissions { get; set; } = new List<TblRolePermission>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
