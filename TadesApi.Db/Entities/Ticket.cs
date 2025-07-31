using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TadesApi.Core;
using TadesApi.Models.ActionsEnum;

namespace TadesApi.Db.Entities
{
    public class Ticket : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public TicketStatus Status { get; set; }

        [Required]
        public TicketPriority Priority { get; set; }

        [Required]
        public TicketCategory Category { get; set; }

        [Required]
        public long CreatedBy { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedByEmail { get; set; }

        public long? AssignedTo { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
    }
}
