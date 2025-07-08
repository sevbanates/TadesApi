using System;
using System.Collections.Generic;
using TadesApi.Core;

namespace TadesApi.Db.Entities;

public partial class SysStringResource : BaseEntity
{
    public int Id { get; set; }

    public string ResourceName { get; set; }

    public string ResourceKey { get; set; }

    public string Value { get; set; }

    public int? SysLanguageId { get; set; }

    public virtual SysLanguage SysLanguage { get; set; }
}
