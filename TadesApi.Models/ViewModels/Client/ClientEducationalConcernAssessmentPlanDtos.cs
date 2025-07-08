using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.Client;

public class AssessmentPlanDto
{
    public long Id { get; set; }
    public Guid GuidId { get; set; }
    public long ClientEducationalConcernId { get; set; }
    public string Details { get; set; }
    public string ConsultantNotes { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string Status { get; set; }
}

public class AssessmentPlanWithFinalSessionDto : AssessmentPlanDto
{
    public ClientSessionDto FinalSession { get; set; }
}

public class CreateAssessmentPlanDto
{
    [Required] [StringLength(1000)] public string Details { get; set; }

    [StringLength(1000)] public string ConsultantNotes { get; set; }
}

public class UpdateAssessmentPlanDto : CreateAssessmentPlanDto
{
    [Required] public long Id { get; set; }

    [Required] public Guid GuidId { get; set; }

    [StringLength(100)] public string Status { get; set; }
}
