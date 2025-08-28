using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ActionsEnum;

public class CreateTicketDto
{
    public string Title { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketCategory Category { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
}

public class TicketMessageDto
{
    public long TicketId { get; set; }

    public long SenderId { get; set; }

    public string SenderName { get; set; }

    public string SenderEmail { get; set; }

    public string SenderType { get; set; } // "user" | "admin"

    public string Message { get; set; }

    public string? Attachments { get; set; } // Dosya yollarý virgül ile ayrýlmýþ

    public DateTime CreatedAt { get; set; }

    public bool IsInternal { get; set; } // Admin'ler arasý notlar için
}
public class CreateTicketMessageDto
{
    public long TicketId { get; set; }

    public long SenderId { get; set; }

    public string SenderName { get; set; }

    public string SenderEmail { get; set; }

    public string SenderType { get; set; } // "user" | "admin"

    public string Message { get; set; }

    public string? Attachments { get; set; } // Dosya yollarý virgül ile ayrýlmýþ

    public DateTime CreatedAt { get; set; }

    public bool IsInternal { get; set; } // Admin'ler arasý notlar için
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
    public string GuidId { get; set; }
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
    public string? SenderName { get; set; }
    public List<TicketMessageDto> Messages { get; set; } = new();
}

public class TicketSearchInput : PagedAndSortedSearchInput
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TicketStatus? TicketStatus { get; set; }
    public TicketPriority? TicketPriority { get; set; }
    public TicketCategory? TicketCategory { get; set; }
}