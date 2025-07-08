using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.PotentialClient;

public class UpsertClientParentDto
{
    [StringLength(100)] [Required] public string FullName { get; set; }

    [StringLength(50)] public string RelationshipToClient { get; set; }

    [Required] [Range(10, 150)] public int Age { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 10)]
    public string Phone { get; set; }

    [Required] public string Address { get; set; }

    [StringLength(100)] public string Occupation { get; set; }

    [StringLength(50)] public string HighestGradeCompleted { get; set; }

    [Required] [StringLength(50)] public string City { get; set; }

    [Required] [StringLength(50)] public string State { get; set; }

    [Required] [StringLength(100)] public string Country { get; set; }

    [StringLength(100)] public string CountryOfOrigin { get; set; }

    [Required] [Range(10000, 99999)] public int Zip { get; set; }
}

public class CreateClientParentDto : UpsertClientParentDto
{
    [Required] [Range(1, int.MaxValue)] public int ClientId { get; set; }

    [Required] public Guid ClientGuid { get; set; }
}

public class UpdateClientParentDto : UpsertClientParentDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}

public class ClientParentDto
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public Guid GuidId { get; set; }
    public string FullName { get; set; }
    public string RelationshipToClient { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Occupation { get; set; }
    public string HighestGradeCompleted { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string CountryOfOrigin { get; set; }
    public int Zip { get; set; }
}