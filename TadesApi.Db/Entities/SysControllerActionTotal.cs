using System;
using System.Collections.Generic;
using TadesApi.Core;

namespace TadesApi.Db.Entities;

public partial class SysControllerActionTotal : BaseEntity
{
    public int Id { get; set; }

    public int Total { get; set; }

    public string Controller { get; set; }

    public int RoleId { get; set; }

    public Guid GuidId { get; set; }

    public DateTime CreDate { get; set; }
}
