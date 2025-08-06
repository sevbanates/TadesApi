using TadesApi.Core;
using TadesApi.Db.PartialEntites;

namespace TadesApi.Db.Entities;

public class AccounterUsers : BaseEntity, ISoftDeletable
{
    public long UserId { get; set; }
    public bool IsDeleted { get; set; }
}