using System;
using System.Collections.Generic;
using TadesApi.Core;

namespace TadesApi.Db.Entities;

public partial class ForgotPassword : BaseEntity
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Email { get; set; }

    public string Token { get; set; }

    public DateTime SendMailDate { get; set; }

    public DateTime? ChangedPassDate { get; set; }

    public Guid GuidId { get; set; }

    public DateTime CreDate { get; set; }
}
