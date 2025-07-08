using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.Client;

public class ClientContactDto
{
    public long Id { get; set; }
    public Guid GuidId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Proximity { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneType { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int ZipCode { get; set; }
    public bool IsEmergency { get; set; }
    public string Comment { get; set; }
    public bool IsPrimary { get; set; }
}

public class UpsertClientContactDto
{
    [Required] [StringLength(50)] public string FirstName { get; set; }

    [Required] [StringLength(50)] public string LastName { get; set; }

    [Required] [StringLength(100)] public string Proximity { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 10)]
    public string PhoneNumber { get; set; }

    [StringLength(50)] public string PhoneType { get; set; }

    [Required] [StringLength(250)] public string Address { get; set; }

    [Required] [StringLength(50)] public string City { get; set; }

    [StringLength(50)] public string State { get; set; }

    [Required] public int ZipCode { get; set; }

    public bool IsPrimary { get; set; }
    public bool IsEmergency { get; set; }

    [StringLength(2000)] public string Comment { get; set; }
}

public class CreateClientContactDto : UpsertClientContactDto
{
    [Required] [Range(1, long.MaxValue)] public long ClientId { get; set; }

    [Required] public Guid ClientGuid { get; set; }
    
    public bool AddAsParent { get; set; }
}

public class UpdateClientContactDto : UpsertClientContactDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}

public class ClientContactViewModel : BaseModel
{
    public Guid? GuidId { get; set; }
    public long? ClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Proximity { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneType { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int? ZipCode { get; set; }
    public bool? IsEmergency { get; set; }
    public string Comment { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreDate { get; set; }
    public long? CreUser { get; set; }
    public DateTime? ModDate { get; set; }
    public long? ModUser { get; set; }

    public string XPhoneType { get; set; }
}