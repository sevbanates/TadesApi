using TadesApi.Core;
using TadesApi.Models.ActionsEnum;

namespace TadesApi.Db.Entities;

public class AccounterRequest : BaseEntity
{
    public long SenderId { get; set; }
    public long TargetId { get; set; }
    public AccounterRequestStatus Status { get; set; }
    public DateTime ModDate { get; set; }

}