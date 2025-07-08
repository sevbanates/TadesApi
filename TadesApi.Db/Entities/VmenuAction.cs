using System;
using System.Collections.Generic;

namespace TadesApi.Db.Entities;

public partial class VmenuAction
{
    public int Id { get; set; }

    public long RoleId { get; set; }

    public string ActionName { get; set; }

    public string Controller { get; set; }

    public string Menu { get; set; }

    public bool? IsChild { get; set; }

    public int? Module { get; set; }
}
