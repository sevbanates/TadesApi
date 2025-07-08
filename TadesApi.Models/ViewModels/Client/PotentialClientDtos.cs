using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.PotentialClient;

public class CreatePotentialClientDto : BaseModel
{
    [Required] [StringLength(50)] 
    public string FirstName { get; set; }
    
    [StringLength(50)]
    public string MiddleName { get; set; }

    [Required] [StringLength(50)]
    public string LastName { get; set; }

    [Required] [StringLength(20)]
    public string BirthDate { get; set; }
    
    [Required] [StringLength(10)]
    public string Gender { get; set; }

    [StringLength(200)]
    public string School { get; set; }
    
    [StringLength(100)]
    public string Grade { get; set; }
    
    [StringLength(250)]
    public string HomeAddress { get; set; }
    
    [Required] [StringLength(100)]
    public string City { get; set; }
    
    [Required] [StringLength(100)]
    public string State { get; set; }
    
    [Required] [StringLength(100)]
    public string Country { get; set; }
    
    [StringLength(100)]
    public string CountryOfOrigin { get; set; }
    
    [Required] [StringLength(20)]
    public string Zip { get; set; }

    [StringLength(20)]
    public string StudentId { get; set; }

    public int? ChronologicalAge { get; set; }
    
    [StringLength(100)]
    public string PrimaryLanguage { get; set; }
    
    [StringLength(100)]
    public string Ethnicity { get; set; }
    
    [StringLength(100)]
    public string PlaceOfBirth { get; set; }
    
    public long? ConsultantId { get; set; }
    
    public DateTime? ConsultantAssignmentDate { get; set; }
}