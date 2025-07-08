using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.CoreHelper;
using TadesApi.Models.Global;
using TadesApi.Models.ViewModels.Client;

namespace TadesApi.Models.ViewModels.ScheduleEvent;

public class ScheduledEventDto
{
    public long Id { get; set; }
    public Guid GuidId { get; set; }
    public string Service { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Repeats { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public ScheduledEventStatus Status { get; set; }
    public string OtherAttendees { get; set; }
    public bool SendIntakeFormLink { get; set; }
    public bool SendRoomLink { get; set; }
    public string StatusUpdateNotes { get; set; }

    public string StatusDescription => Status.GetName();

}

public class ScheduledEventWithClientsDto : ScheduledEventDto
{
    public List<ClientBasicDto> Clients { get; set; }
    public string RoomLink => Clients?.FirstOrDefault()?.RoomLink;
    
}

public class ScheduledEventWithConsultantDto : ScheduledEventDto
{
    public UserBasicDto Consultant { get; set; }
}

public class ScheduledEventWithClientsAndConsultantDto : ScheduledEventDto
{
    public List<ClientBasicDto> Clients { get; set; }
    public UserBasicDto Consultant { get; set; }
    public string RoomLink => Clients?.FirstOrDefault()?.RoomLink;
    public long? ClientId => Clients?.FirstOrDefault()?.Id;
}

public class CreateScheduleEventDto
{
    [Required] [StringLength(100)] public string Service { get; set; }

    [StringLength(500)] public string Title { get; set; }

    [DataType(DataType.Date)] public DateTime Date { get; set; }

    [StringLength(50)] public string StartTime { get; set; }

    [StringLength(50)] public string EndTime { get; set; }
    public bool IsAllDayEvent { get; set; }

    [StringLength(100)] public string Repeats { get; set; }

    [StringLength(500)] public string Location { get; set; }

    [StringLength(2000)] public string Description { get; set; }

    public long? ClientId { get; set; }
    public bool SendIntakeFormLink { get; set; }
    public bool SendRoomLink { get; set; }
    public string OtherAttendees { get; set; }
}

public class UpdateScheduleEventDto : CreateScheduleEventDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}

public class ScheduledEventSearchInput : PagedAndSortedSearchInput
{
    public ScheduledEventStatus? Status { get; set; } = ScheduledEventStatus.Scheduled;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? ConsultantId { get; set; }
}

public class UpdateScheduledEventStatusDto
{
    [Required] public Guid GuidId { get; set; }

    [Required] public ScheduledEventStatus Status { get; set; }

    [StringLength(500)] public string Message { get; set; }
}