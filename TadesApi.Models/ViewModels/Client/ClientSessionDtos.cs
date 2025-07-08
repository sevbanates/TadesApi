using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;

namespace TadesApi.Models.ViewModels.Client;

public class ClientSessionDto
{
    public long Id { get; set; }
    public Guid GuidId { get; set; }
    public long ClientId { get; set; }
    public DateTime Date { get; set; }
    public string StartTime { get; set; }
    public int Duration { get; set; }
    public string Notes { get; set; }
    public ClientSessionAssessmentPlanStatusUpdateDto AssessmentPlanStatusChanges { get; set; }
    public ClientBasicDto Client { get; set; }
    public UserBasicDto Consultant { get; set; }
}

public class UpsertClientSessionDto
{
    [Required] [DataType(DataType.Date)] public DateTime Date { get; set; }

    [Required] [StringLength(50)] public string StartTime { get; set; }

    [Required] public int Duration { get; set; }


    [StringLength(2000)] public string Notes { get; set; }

    public List<AssessmentPlanWithStatus> AssessmentPlansWithStatus { get; set; } = new();
}

public class CreateClientSessionDto : UpsertClientSessionDto
{
    [Required] [Range(1, long.MaxValue)] public long ClientId { get; set; }

    [Required] public Guid ClientGuid { get; set; }
}

public class AssessmentPlanWithStatus
{
    public long Id { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
}

public class UpdateClientSessionDto : UpsertClientSessionDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}

public class ClientSessionAssessmentPlanStatusUpdateDto
{
    public long SessionId { get; set; }
    public long AssessmentPlanId { get; set; }
    public string OldStatus { get; set; }
    public string NewStatus { get; set; }
    public string Notes { get; set; }
}