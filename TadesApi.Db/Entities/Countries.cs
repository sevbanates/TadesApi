using TadesApi.Core;

namespace TadesApi.Db.Entities;

public class Countries: BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string PhoneCode { get; set; }
}