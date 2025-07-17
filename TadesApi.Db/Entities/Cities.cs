using TadesApi.Core;

namespace TadesApi.Db.Entities;

public class Cities: BaseEntity
{
    public string Name { get; set; }
    public long CountryId { get; set; }
}