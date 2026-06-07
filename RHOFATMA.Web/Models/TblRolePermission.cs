using System;
using System.Collections.Generic;

namespace RHOFATMA.Web.Models;

public partial class TblRolePermission
{
    public int IdRolePermission { get; set; }

    public int IdRole { get; set; }

    public int IdPermission { get; set; }

    public virtual TblPermission IdPermissionNavigation { get; set; } = null!;

    public virtual TblRole IdRoleNavigation { get; set; } = null!;
}
