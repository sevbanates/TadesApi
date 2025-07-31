using System;
using System.Collections.Generic;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ActionsEnum;

public class CreateTicketDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketCategory Category { get; set; }
    public string Email { get; set; }
}

public class TicketMessageDto
{
    public long TicketId { get; set; }
    public string Message { get; set; }
    public bool IsInternal { get; set; }
    public List<string>? Attachments { get; set; }
}

public class TicketResponseDto
{
    public string Message { get; set; }
    public bool IsInternal { get; set; }
}

public class UpdateTicketDto : IBaseUpdateModel
{
    public long Id { get; set; }
    public Guid GuidId { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public TicketStatus? Status { get; set; }
    public TicketPriority? Priority { get; set; }
    public TicketCategory? Category { get; set; }
    public long? AssignedTo { get; set; }
}

public class TicketDto : BaseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketCategory Category { get; set; }
    public long CreatedBy { get; set; }
    public string CreatedByEmail { get; set; }
    public long? AssignedTo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<TicketMessageDto> Messages { get; set; } = new();
}