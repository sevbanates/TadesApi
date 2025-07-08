using System;
using System.Collections.Generic;

namespace TadesApi.Db.Entities;

public partial class SysControllerAction
{
    public int Id { get; set; }

    public int ControllerId { get; set; }

    public string ControllerName { get; set; }

    public int ActionNo { get; set; }

    public string ActionName { get; set; }

    public Guid GuidId { get; set; }

    public DateTime CreDate { get; set; }
}
