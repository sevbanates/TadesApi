using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.PotentialClient;

public class UpsertClientSiblingDto
{
    [Required] [StringLength(100)] public string FullName { get; set; }

    [Required] [Range(0, 100)] public int Age { get; set; }
}

public class CreateClientSiblingDto : UpsertClientSiblingDto
{
    [Required] [Range(1, int.MaxValue)] public long ClientId { get; set; }

    [Required] public Guid ClientGuid { get; set; }
}

public class UpdateClientSiblingDto : UpsertClientSiblingDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}

public class ClientSiblingDto
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public Guid GuidId { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public DateTime CreDate { get; set; }
}