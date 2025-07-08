using System;
using System.Collections.Generic;
using TadesApi.Core;

namespace TadesApi.Db.Entities;

public partial class SysControllerActionRole : BaseEntity
{
    public int Id { get; set; }

    public long RoleId { get; set; }

    public string ActionName { get; set; }

    public int ActionNo { get; set; }

    public string Controller { get; set; }

    //public Guid GuidId { get; set; }

    //public DateTime CreDate { get; set; }
}
