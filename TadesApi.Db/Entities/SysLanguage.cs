using System;
using System.Collections.Generic;
using TadesApi.Core;

namespace TadesApi.Db.Entities;

public partial class SysLanguage : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Culture { get; set; }

    public Guid GuidId { get; set; }

    public DateTime CreDate { get; set; }

    public virtual ICollection<SysStringResource> SysStringResource { get; set; } = new List<SysStringResource>();
}
