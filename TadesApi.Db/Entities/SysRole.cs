using System;
using System.Collections.Generic;
using TadesApi.Db.Entities;

namespace TadesApi.Db.Entities;

public partial class SysRole
{
    public string RoleName { get; set; }

    public string RoleDescription { get; set; }

    public int Id { get; set; }

    public Guid GuidId { get; set; }

    public DateTime CreDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
