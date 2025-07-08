using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.Client;

public class ClientEducationalConcernDto
{
    public long Id { get; set; }
    public Guid GuidId { get; set; }
    public long ClientId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

public class ClientEducationalConcernWithAssessmentPlansDto : ClientEducationalConcernDto
{
    public List<AssessmentPlanDto> AssessmentPlans { get; set; }
}

public class UpsertClientEducationalConcernDto
{
    [Required] [StringLength(250)] public string Title { get; set; }

    [Required] [StringLength(2000)] public string Description { get; set; }

    [StringLength(4000)] public string ConsultantNotes { get; set; }

    public List<CreateAssessmentPlanDto> AssessmentPlans { get; set; }
}

public class CreateClientEducationalConcernDto : UpsertClientEducationalConcernDto
{
    [Required] [Range(1, long.MaxValue)] public long ClientId { get; set; }

    [Required] public Guid ClientGuid { get; set; }
}

public class UpdateClientEducationalConcernDto : UpsertClientEducationalConcernDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}